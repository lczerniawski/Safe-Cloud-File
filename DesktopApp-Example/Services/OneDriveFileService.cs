using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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
            var auth = _graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync().Result;
        }

        public async Task<ShareLinksDto> UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey,bool isShared)
        {
            using (var inputStream = new MemoryStream())
            using (var outputStream = new MemoryStream())
            {
                fileStream.CopyTo(inputStream);
                var userKeys = SafeCloudFile.Encrypt(inputStream,outputStream, receivers);

                var fileSign = SafeCloudFile.SignFile(outputStream, senderKey);
                var fileData = new FileData(fileSign, userKeys, new SenderPublicKey(senderKey.Exponent, senderKey.Modulus), fileName + fileExtension);
                var fileDataJson = JsonConvert.SerializeObject(fileData);

                var uploadedJsonFileDto = await UploadJson(fileName, fileDataJson.GenerateStream(), isShared);
                var uploadedFileShareLink = await UploadFile(fileName, fileExtension, outputStream, uploadedJsonFileDto.Id, isShared);

                return new ShareLinksDto(uploadedFileShareLink, uploadedJsonFileDto.ShareLink);
            }
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

        public async Task<MemoryStream> DownloadFile(ViewFile file, string receiverEmail, RSAParameters receiverKey)
        {
            var decryptedStream = new MemoryStream();
            var jsonFileData = await _graphServiceClient.Me.Drive.Special.AppRoot
                .ItemWithPath(file.Name.Split('.').First() + ".json")
                .Content.Request().GetAsync() as MemoryStream;
            if (jsonFileData == null)
                throw new Exception("Error while downloading json file!");

            var fileData = FileDataHelpers.DownloadFileData(jsonFileData, receiverEmail);

            var encryptedStream = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name).Content
                .Request()
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

            SafeCloudFile.Decrypt(encryptedStream, decryptedStream, fileData.UserKeys[receiverEmail], receiverKey);

            jsonFileData.Close();
            encryptedStream.Close();

            return decryptedStream;
        }

        public async Task<SharedDownload> DownloadShared(string encryptedFileLink, string jsonFileLink, string receiverEmail, RSAParameters receiverKey)
        {
            using (var encryptedStream = new MemoryStream())
            using (var jsonFileData = new MemoryStream())
            {
                var decryptedStream = new MemoryStream();
                await GetFileStream(jsonFileLink,jsonFileData);
                var fileData = FileDataHelpers.DownloadFileData(jsonFileData, receiverEmail);

                await GetFileStream(encryptedFileLink,encryptedStream);

                var senderKey = new RSAParameters
                {
                    Exponent = fileData.SenderPublicKey.Expontent,
                    Modulus = fileData.SenderPublicKey.Modulus
                };
                var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
                if (!isValid)
                    throw new Exception("Invalid file sign!");

                SafeCloudFile.Decrypt(encryptedStream,decryptedStream, fileData.UserKeys[receiverEmail], receiverKey);


                return new SharedDownload(decryptedStream, fileData.FileName);
            }
        }

        public async Task DeleteFile(ViewFile file)
        {
            await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name.Split('.').First() + ".json")
                .Request().DeleteAsync();

            await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name).Request().DeleteAsync();
        }

        private async Task<string> UploadFile(string fileName, string fileExtension, Stream stream, string jsonFileId, bool isShared)
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

            if (isShared)
                return await SetPermissions(uploadedFile?.Name);

            return null;
        }

        private async Task<UploadJsonDto> UploadJson(string fileName,Stream stream, bool isShared)
        {
            var result = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(fileName+".json").Content.Request().PutAsync<DriveItem>(stream);

            if (isShared)
                return new UploadJsonDto(result.Id, await SetPermissions(fileName+".json"));

            return new UploadJsonDto(result.Id, null);
        }

        public async Task<string> SetPermissions(string fileName)
        {
            var type = "view";
            var scope = "anonymous";
            var response = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(fileName).CreateLink(type,scope).Request().PostAsync();

            return response.Link.WebUrl;
        }

        private async Task GetFileStream(string fileLink, Stream outputStream)
        {
            string base64Value = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fileLink));
            string encodedUrl = "u!" + base64Value.TrimEnd('=').Replace('/','_').Replace('+','-');

            var stream = await _graphServiceClient.Shares[encodedUrl].DriveItem.Content.Request().GetAsync();
            await stream.CopyToAsync(outputStream);
            stream.Dispose();
        }
    }
}
