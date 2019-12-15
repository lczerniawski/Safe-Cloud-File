using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey)
        {
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);

            var encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);
            memoryStream.Dispose();

            var fileSign = SafeCloudFile.SignFile(encryptedFile.EncryptedStream, senderKey);
            var fileData = new FileData(fileSign,encryptedFile.UserKeys);
            var fileDataJson = JsonConvert.SerializeObject(fileData);

            var uploadedJsonFileId = await UploadJson(fileName, fileDataJson.GenerateStream());
            await UploadFile(fileName, fileExtension, encryptedFile.EncryptedStream,uploadedJsonFileId);
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

        public async Task DownloadFile(string path, ViewFile file, string receiverEmail, RSAParameters receiverKey, RSAParameters senderKey)
        {
            using (var fileStream = new FileStream(path + "/" + file.Name, FileMode.Create))
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

                encryptedStream.Position = 0;
                var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
                if (!isValid)
                    throw new Exception("Invalid file sign!");

                var decryptedStream =
                    SafeCloudFile.Decrypt(encryptedStream, fileData.UserKeys[receiverEmail], receiverKey);
                decryptedStream.Position = 0;
                await decryptedStream.CopyToAsync(fileStream);

                jsonFileData.Close();
                encryptedStream.Close();
                decryptedStream.Close();
            }
        }

        public async Task DeleteFile(ViewFile file)
        {
            await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name.Split('.').First() + ".json")
                .Request().DeleteAsync();

            await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(file.Name).Request().DeleteAsync();
        }

        private async Task UploadFile(string fileName, string fileExtension, Stream stream, string jsonFileId)
        {
            DriveItem uploadedFile = null;
            var uploadSession = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(fileName+fileExtension).CreateUploadSession().Request().PostAsync();
            if (uploadSession != null)
            {
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
            }
        }

        private async Task<string> UploadJson(string fileName,Stream stream)
        {
            var result = await _graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(fileName+".json").Content.Request().PutAsync<DriveItem>(stream);

            return result.Id;
        }
    }
}
