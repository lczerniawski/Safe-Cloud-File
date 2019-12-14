using System;
using System.Collections.Generic;
using System.IO;
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

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, _scopes, "user",
                CancellationToken.None, new FileDataStore(credentialPath, true)).Result;

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        public async Task UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey)
        {
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);

            var encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);
            memoryStream.Dispose();

            var fileSign = SafeCloudFile.SignFile(encryptedFile.EncryptedStream, senderKey);
            var fileData = new FileData(fileSign,encryptedFile.UserKeys);
            var fileDataJson = JsonConvert.SerializeObject(fileData);

            var uploadJsonFileDto = await UploadJson(fileName, fileDataJson.GenerateStream());
            if(uploadJsonFileDto.UploadStatus != UploadStatus.Completed)
                throw new Exception("Error while uploading file!");

            var uploadFileStatus = await UploadFile(fileName, fileExtension, encryptedFile.EncryptedStream,uploadJsonFileDto.Id);
            if(uploadFileStatus != UploadStatus.Completed)
                throw new Exception("Error while uploading file!");
        }

        public async Task<List<ViewFile>> GetAllFiles()
        {
            List<ViewFile> result = new List<ViewFile>();

            FilesResource.ListRequest request = _driveService.Files.List();
            request.Fields = "*";
            do {
                FileList files = await request.ExecuteAsync();
                foreach (var filesFile in files.Files)
                {
                    if(!filesFile.Name.Contains(".json"))
                        result.Add(new ViewFile(filesFile.Id,filesFile.Name,filesFile.AppProperties?["JsonFileId"]));
                }
                request.PageToken = files.NextPageToken;
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

        public async Task DownloadFile(string path,ViewFile file,string receiverEmail,RSAParameters receiverKey, RSAParameters senderKey)
        {
            using (var fileStream = new FileStream(path + "/" + file.Name, FileMode.Create))
            {
                using (var encryptedStream = new MemoryStream())
                {
                    using (var jsonFileData = new MemoryStream())
                    {
                        var jsonFileDownloadProgress = await _driveService.Files.Get(file.JsonFileId).DownloadAsync(jsonFileData);
                        if(jsonFileDownloadProgress.Status != DownloadStatus.Completed)
                            throw new Exception("Error while downloading json file!");

                        jsonFileData.Position = 0;
                        var streamReader = new StreamReader(jsonFileData);
                        var jsonString = streamReader.ReadToEnd();
                        var fileData = JsonConvert.DeserializeObject<FileData>(jsonString);
                        if(!fileData.UserKeys.ContainsKey(receiverEmail))
                            throw new Exception("User can't decrypt this file!");

                        var encryptedFileDownloadProgress = await _driveService.Files.Get(file.Id).DownloadAsync(encryptedStream);
                        if(encryptedFileDownloadProgress.Status != DownloadStatus.Completed)
                            throw new Exception("Error while downloading encrypted file!");

                        encryptedStream.Position = 0;
                        var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
                        if(!isValid)
                            throw new Exception("Invalid file sign!");

                        var decryptedStream = SafeCloudFile.Decrypt(encryptedStream,fileData.UserKeys[receiverEmail],receiverKey);
                        decryptedStream.Position = 0;
                        await decryptedStream.CopyToAsync(fileStream);
                    }   
                }
            }
        }

        public async Task DeleteFile(ViewFile file)
        {
            await _driveService.Files.Delete(file.Id).ExecuteAsync();
            await _driveService.Files.Delete(file.JsonFileId).ExecuteAsync();
        }

        private async Task<UploadStatus> UploadFile(string fileName,string fileExtension,Stream stream,string jsonFileId)
        {
            var encryptedFileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName + fileExtension,
                AppProperties = new Dictionary<string, string>
                {
                    {"JsonFileId",jsonFileId}
                }
            };

            var mimeType = MimeTypesMap.GetMimeType(fileName + fileExtension);

            var encryptedFileRequest = _driveService.Files.Create(encryptedFileMetadata, stream ,mimeType);
            encryptedFileRequest.Fields = "id";
            var result = await encryptedFileRequest.UploadAsync();

            return result.Status;
        }

        private async Task<UploadFileDto> UploadJson(string fileName,Stream stream)
        {
            var encryptedFileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = $"{fileName}.json"
            };

            var mimeType = MimeTypesMap.GetMimeType($"{fileName}.json");

            var jsonFileRequest = _driveService.Files.Create(encryptedFileMetadata, stream ,mimeType);
            jsonFileRequest.Fields = "id";
            var result = await jsonFileRequest.UploadAsync();

            return new UploadFileDto(jsonFileRequest.ResponseBody.Id, result.Status);
        }
    }
}
