using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;
using DesktopApp_Example.Helpers;
using Google.Apis.Download;
using Google.Apis.Upload;
using HeyRed.Mime;
using Inzynierka_Core;
using Inzynierka_Core.Model;
using Microsoft.Graph;
using Newtonsoft.Json;
using File = Microsoft.Graph.File;

namespace DesktopApp_Example.Services
{
    public class OneDriveFileService : IFileService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public OneDriveFileService()
        {
            _graphServiceClient = MicrosoftAuthenticationHelper.GetAuthenticatedClient();
        }

        public async Task<ShareLinksDto> UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey)
        {
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);

            var encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);
            memoryStream.Dispose();

            var fileSign = SafeCloudFile.SignFile(encryptedFile.EncryptedStream, senderKey);
            var fileData = new FileData(fileSign,encryptedFile.UserKeys,new SenderPublicKey(senderKey.Exponent,senderKey.Modulus),fileName+fileExtension);
            var fileDataJson = JsonConvert.SerializeObject(fileData);

            var uploadedJsonFileDto = await UploadJson(fileName, fileDataJson.GenerateStream());
            var uploadedFileShareLink = await UploadFile(fileName, fileExtension, encryptedFile.EncryptedStream,uploadedJsonFileDto.Id);

            return new ShareLinksDto(uploadedFileShareLink,uploadedJsonFileDto.ShareLink);
        }

        public async Task<List<ViewFile>> GetAllFiles()
        {
            List<ViewFile> result = new List<ViewFile>();
            var driveFiles = await _graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();
            foreach (var driveFile in driveFiles)
            {
                if(!driveFile.Name.Contains(".json"))
                    result.Add(new ViewFile(driveFile.Id,driveFile.Name,null));
            }

            return result;
        }

        public async Task<MemoryStream> DownloadFile(string path, ViewFile file, string receiverEmail, RSAParameters receiverKey)
        {
            var jsonFileData = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name.Split('.').First()+".json")
                    .Content.Request().GetAsync() as MemoryStream;
                if (jsonFileData == null)
                    throw new Exception("Error while downloading json file!");

                jsonFileData.Position = 0;
                var streamReader = new StreamReader(jsonFileData);
                var jsonString = streamReader.ReadToEnd();
                var fileData = JsonConvert.DeserializeObject<FileData>(jsonString);
                if (!fileData.UserKeys.ContainsKey(receiverEmail))
                    throw new Exception("User can't decrypt this file!");

                var encryptedStream =
                    await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name).Content.Request()
                        .GetAsync() as MemoryStream;

                if (encryptedStream == null)
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

                jsonFileData.Close();
                encryptedStream.Close();

                return decryptedStream;
        }

        public async Task<SharedDownload> DownloadShared(string encryptedFileLink, string jsonFileLink, string receiverEmail, RSAParameters receiverKey)
        {
            var jsonStream = await GetFileStream(jsonFileLink);
            if (jsonStream == null)
                throw new Exception("Error while downloading json file!");

            var streamReader = new StreamReader(jsonStream);
            var jsonString = streamReader.ReadToEnd();
            var fileData = JsonConvert.DeserializeObject<FileData>(jsonString);
            if (!fileData.UserKeys.ContainsKey(receiverEmail))
                throw new Exception("User can't decrypt this file!");

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
            await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name.Split('.').First() + ".json")
                .Request().DeleteAsync();

            await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name).Request().DeleteAsync();
        }

        private async Task<string> UploadFile(string fileName, string fileExtension, Stream stream, string jsonFileId)
        {
            DriveItem uploadedFile = null;
            var uploadSession = await _graphServiceClient.Me.Drive.Special.AppRoot
                .ItemWithPath(fileName + fileExtension).CreateUploadSession().Request().PostAsync();

            // Chunk size must be divisible by 320KiB, our chunk size will be slightly more than 1MB 
            int maxSizeChunk = (320 * 1024) * 4;
            ChunkedUploadProvider uploadProvider =
                new ChunkedUploadProvider(uploadSession, _graphServiceClient, stream, maxSizeChunk);
            var chunkRequests = uploadProvider.GetUploadChunkRequests();
            var exceptions = new List<Exception>();

            foreach (var request in chunkRequests)
            {
                var result = await uploadProvider.GetChunkRequestResponseAsync(request, exceptions);

                if (result.UploadSucceeded)
                {
                    uploadedFile = result.ItemResponse;
                }
            }

            var type = "view";
            var scope = "anonymous";
            var shareLinkResponse = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(uploadedFile?.Name)
                .CreateLink(type, scope).Request().PostAsync();

            return shareLinkResponse.Link.WebUrl;
        }

        private async Task<UploadJsonDto> UploadJson(string fileName,Stream stream)
        {
            var result = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(fileName+".json").Content.Request().PutAsync<DriveItem>(stream);

            var type = "view";
            var scope = "anonymous";
            var response = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(fileName+".json").CreateLink(type,scope).Request().PostAsync();

            return new UploadJsonDto(result.Id, response.Link.WebUrl);
        }

        private async Task<Stream> GetFileStream(string fileLink)
        {
            string base64Value = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fileLink));
            string encodedUrl = "u!" + base64Value.TrimEnd('=').Replace('/','_').Replace('+','-');

            var stream = await _graphServiceClient.Shares[encodedUrl].DriveItem.Content.Request().GetAsync();

            return stream;
        }
    }
}
