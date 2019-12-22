using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;
using DesktopApp_Example.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using HeyRed.Mime;
using Inzynierka_Core;
using Inzynierka_Core.Model;
using Newtonsoft.Json;
using File = System.IO.File;

namespace DesktopApp_Example.Services
{
    public class GoogleDriveFileService : IFileService
    {
        private readonly string[] _scopes = { DriveService.Scope.DriveFile };
        private readonly string _applicationName = "Drive API DesktopApp-Example";
        private readonly DriveService _driveService;

        public GoogleDriveFileService()
        {
            var credentialPath = "token.json";
            var clientSecrets = new ClientSecrets
            {
                ClientId = "158037173377-tdaq0rn5eha2lcg2p1d06nmcgg2ishui.apps.googleusercontent.com",
                ClientSecret = "C4L0BQGRM9KE6vKWeBcgTlEi"
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            CancellationToken ct = cts.Token;

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, _scopes, "user",
                ct, new FileDataStore(credentialPath, true)).Result;

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        public async Task<ShareLinksDto> UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey, bool isShared)
        {
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);

            var encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);
            memoryStream.Dispose();

            var fileSign = SafeCloudFile.SignFile(encryptedFile.EncryptedStream, senderKey);
            var fileData = new FileData(fileSign,encryptedFile.UserKeys,new SenderPublicKey(senderKey.Exponent,senderKey.Modulus),fileName+fileExtension);
            var fileDataJson = JsonConvert.SerializeObject(fileData);

            var uploadJsonFileDto = await UploadJson(fileName, fileDataJson.GenerateStream(),isShared);
            if(uploadJsonFileDto == null)
                throw new Exception("Error while uploading file!");

            var uploadedFileLink = await UploadFile(fileName, fileExtension, encryptedFile.EncryptedStream,uploadJsonFileDto.Id,isShared);
            if(uploadedFileLink == null)
                throw new Exception("Error while uploading file!");

            return new ShareLinksDto(uploadedFileLink,uploadJsonFileDto.ShareLink);
        }

        public async Task<List<ViewFile>> GetAllFiles()
        {
            List<ViewFile> result = new List<ViewFile>();

            FilesResource.ListRequest request = _driveService.Files.List();
            request.Fields = "*";
            do {
                FileList fileList = await request.ExecuteAsync();
                foreach (var file in fileList.Files)
                {
                    if(!file.Name.Contains(".json"))
                        result.Add(new ViewFile(file.Id,file.Name,file.AppProperties?["JsonFileId"]));
                }
                request.PageToken = fileList.NextPageToken;
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

        public async Task<MemoryStream> DownloadFile(string path,ViewFile file,string receiverEmail,RSAParameters receiverKey)
        {
            using (var encryptedStream = new MemoryStream())
            {
                using (var jsonFileData = new MemoryStream())
                {
                    var jsonFileDownloadProgress =
                        await _driveService.Files.Get(file.JsonFileId).DownloadAsync(jsonFileData);
                    if (jsonFileDownloadProgress.Status != DownloadStatus.Completed)
                        throw new Exception("Error while downloading json file!");

                    var fileData = FileDataHelpers.DownloadFileData(jsonFileData, receiverEmail);

                    var encryptedFileDownloadProgress =
                        await _driveService.Files.Get(file.Id).DownloadAsync(encryptedStream);
                    if (encryptedFileDownloadProgress.Status != DownloadStatus.Completed)
                        throw new Exception("Error while downloading encrypted file!");

                    var senderKey = new RSAParameters
                    {
                        Exponent = fileData.SenderPublicKey.Expontent,
                        Modulus = fileData.SenderPublicKey.Modulus
                    };
                    encryptedStream.Position = 0;
                    var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
                    if (!isValid)
                        throw new Exception("Invalid file sign!");

                    var decryptedStream =
                        SafeCloudFile.Decrypt(encryptedStream, fileData.UserKeys[receiverEmail], receiverKey);

                    return decryptedStream;
                }
            }
            
        }

        public async Task<SharedDownload> DownloadShared(string encryptedFileLink, string jsonFileLink,string receiverEmail, RSAParameters receiverKey)
        {
            var jsonStream = await GetFileStream(jsonFileLink);
            if (jsonStream == null)
                throw new Exception("Error while downloading json file!");

            var fileData = FileDataHelpers.DownloadFileData(jsonStream, receiverEmail);

            var encryptedStream = await GetFileStream(encryptedFileLink) as MemoryStream;
            if (encryptedStream == null)
                throw new Exception("Error while downloading encrypted file!");

            var senderKey = new RSAParameters
            {
                Exponent = fileData.SenderPublicKey.Expontent,
                Modulus = fileData.SenderPublicKey.Modulus
            };
            var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
            if (!isValid)
                throw new Exception("Invalid file sign!");

            var decryptedStream =
                SafeCloudFile.Decrypt(encryptedStream, fileData.UserKeys[receiverEmail], receiverKey);

            jsonStream.Close();
            encryptedStream.Close();

            return new SharedDownload(decryptedStream,fileData.FileName);
        }

        public async Task DeleteFile(ViewFile file)
        {
            await _driveService.Files.Delete(file.Id).ExecuteAsync();
            await _driveService.Files.Delete(file.JsonFileId).ExecuteAsync();
        }

        private async Task<string> UploadFile(string fileName,string fileExtension,Stream stream,string jsonFileId,bool isShared)
        {
            var encryptedFileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName + fileExtension,
                AppProperties = new Dictionary<string, string>
                {
                    {"JsonFileId",jsonFileId}
                }
            };

            var encryptedFileRequest = await Upload(fileName, encryptedFileMetadata, stream,isShared);

            return encryptedFileRequest.ResponseBody.WebContentLink;
        }

        private async Task<UploadJsonDto> UploadJson(string fileName,Stream stream,bool isShared)
        {
            var jsonFileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = $"{fileName}.json"
            };

            var jsonFileRequest = await Upload(fileName, jsonFileMetadata, stream,isShared);

            return new UploadJsonDto(jsonFileRequest.ResponseBody.Id, jsonFileRequest.ResponseBody.WebContentLink);
        }

        private async Task<FilesResource.CreateMediaUpload> Upload(string fileName,Google.Apis.Drive.v3.Data.File fileMetadata, Stream stream,bool isShared)
        {
            var mimeType = MimeTypesMap.GetMimeType($"{fileName}.json");

            var fileRequest = _driveService.Files.Create(fileMetadata, stream ,mimeType);
            fileRequest.Fields = "*";
            var result = await fileRequest.UploadAsync();
            if(result.Status != UploadStatus.Completed)
                throw new Exception("Error while uploading json file!");

            if (isShared)
                await SetPermissions(fileRequest.ResponseBody.Id);

            return fileRequest;
        }

        private async Task SetPermissions(string fileId)
        {
            Permission userPermission = new Permission();
            userPermission.Type = "anyone";
            userPermission.Role = "reader";
            var permissionResult = await _driveService.Permissions.Create(userPermission,fileId).ExecuteAsync();
            if(permissionResult.Id == null)
                throw new Exception("Error while setting permissions to file!");
        }

        private async Task<Stream> GetFileStream(string fileLink)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(fileLink);
                var stream = await result.Content.ReadAsStreamAsync();

                return stream;
            }
        }
    }
}
