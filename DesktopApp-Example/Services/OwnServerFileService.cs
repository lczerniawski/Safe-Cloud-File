using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;
using DesktopApp_Example.Helpers;
using Google.Apis.Download;
using Google.Apis.Upload;
using Inzynierka_Core;
using Inzynierka_Core.Model;
using Newtonsoft.Json;

namespace DesktopApp_Example.Services
{
    public class OwnServerFileService : IFileService
    {
        private readonly AuthData _authData;
        private readonly string BaseUrl = "http://localhost:57640";

        public OwnServerFileService(AuthData authData)
        {
            _authData = authData;
        }

        public async Task<ShareLinksDto> UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey,bool isShared)
        {
            using (var inputStream = new MemoryStream())
            using (var outputStream = new MemoryStream())
            {
                fileStream.CopyTo(inputStream);

                var userKeys = SafeCloudFile.Encrypt(inputStream,outputStream, receivers);
                inputStream.Dispose();

                var fileSign = SafeCloudFile.SignFile(outputStream, senderKey);
                var fileData = new FileData(fileSign, userKeys,
                    new SenderPublicKey(senderKey.Exponent, senderKey.Modulus), fileName + fileExtension);
                var fileDataJson = JsonConvert.SerializeObject(fileData);

                var uploadJsonFileDto = await UploadJson(fileName, fileDataJson.GenerateStream(), isShared);
                if (uploadJsonFileDto == null)
                    throw new Exception("Error while uploading file!");

                var uploadFile = await UploadFile(fileName, fileExtension, outputStream,uploadJsonFileDto.Id, isShared);
                if (uploadFile == null)
                    throw new Exception("Error while uploading file!");

                return new ShareLinksDto(uploadFile.ShareLink, uploadJsonFileDto.ShareLink);
            }
        }

        public async Task<List<ViewFile>> GetAllFiles()
        {
            List<ViewFile> result = new List<ViewFile>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);
                var response = await client.GetAsync($"{BaseUrl}/api/file");
                var responseString = await response.Content.ReadAsStringAsync();

                var fileList = JsonConvert.DeserializeObject<IEnumerable<FileDto>>(responseString);
                foreach (var file in fileList)
                {
                    if(!file.FileType.Contains(".json"))
                        result.Add(new ViewFile(file.Id,file.FileName + file.FileType,file.JsonFileId));
                }

                return result;
            }
        }

        public async Task<MemoryStream> DownloadFile(ViewFile file, string receiverEmail, RSAParameters receiverKey)
        {
            using (var encryptedStream = new MemoryStream())
            using (var jsonFileData = new MemoryStream())
            {
                var decryptedStream = new MemoryStream();
                await DownloadFile(file.JsonFileId,jsonFileData);

                var fileData = FileDataHelpers.DownloadFileData(jsonFileData, receiverEmail);

                await DownloadFile(file.Id, encryptedStream);
                var senderKey = new RSAParameters
                {
                    Exponent = fileData.SenderPublicKey.Expontent,
                    Modulus = fileData.SenderPublicKey.Modulus
                };
                var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
                if (!isValid)
                    throw new Exception("Invalid file sign!");

                SafeCloudFile.Decrypt(encryptedStream, decryptedStream, fileData.UserKeys[receiverEmail], receiverKey);

                return decryptedStream;
            }
        }

        public async Task<SharedDownload> DownloadShared(string encryptedFileLink, string jsonFileLink,string receiverEmail, RSAParameters receiverKey)
        {
            using (var jsonStream = new MemoryStream())
            using (var encryptedStream = new MemoryStream())
            {
                var decryptedStream = new MemoryStream();
                await GetFileStream(jsonFileLink,jsonStream);

                var fileData = FileDataHelpers.DownloadFileData(jsonStream, receiverEmail);

                await GetFileStream(encryptedFileLink,encryptedStream);

                var senderKey = new RSAParameters
                {
                    Exponent = fileData.SenderPublicKey.Expontent,
                    Modulus = fileData.SenderPublicKey.Modulus
                };
                var isValid = SafeCloudFile.VerifySignedFile(encryptedStream, fileData.FileSign, senderKey);
                if (!isValid)
                    throw new Exception("Invalid file sign!");

                SafeCloudFile.Decrypt(encryptedStream, decryptedStream, fileData.UserKeys[receiverEmail], receiverKey);

                jsonStream.Close();
                encryptedStream.Close();

                return new SharedDownload(decryptedStream, fileData.FileName);
            }
        }

        public async Task DeleteFile(ViewFile file)
        {
            await DeleteFile(file.JsonFileId);
            await DeleteFile(file.Id);
        }

        private async Task<FileDto> UploadFile(string fileName,string fileExtension,Stream stream,string jsonFileId,bool isShared)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);
                stream.Position = 0;

                var contentfile = new StreamContent(stream);
                content.Add(contentfile, "FormFile", fileName + fileExtension);
                content.Add(new StringContent(fileName),"FileName");
                content.Add(new StringContent(fileExtension),"FileType");
                content.Add(new StringContent(jsonFileId),"JsonFileId");
                content.Add(new StringContent(isShared.ToString()),"IsShared");
                var result = await client.PostAsync($"{BaseUrl}/api/file", content);
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while uploading file!");

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<FileDto>(resultString);
            }
        }

        private async Task<FileDto> UploadJson(string fileName,Stream stream, bool isShared)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);
                stream.Position = 0;

                var contentfile = new StreamContent(stream);
                content.Add(contentfile, "FormFile", $"{fileName}.json");
                content.Add(new StringContent(fileName),"FileName");
                content.Add(new StringContent(".json"),"FileType");
                content.Add(new StringContent(isShared.ToString()),"IsShared");
                var result = await client.PostAsync($"{BaseUrl}/api/file", content);
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while uploading file!");

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<FileDto>(resultString);
            }
        }

        private async Task DownloadFile(string fileId, Stream outputStream)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);

                var result = await client.GetAsync($"{BaseUrl}/api/file/{fileId}");
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while downloading file!");

                var stream = await result.Content.ReadAsStreamAsync();
                await stream.CopyToAsync(outputStream);
                stream.Dispose();
            }
        }

        private async Task DeleteFile(string fileId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);

                var result = await client.DeleteAsync($"{BaseUrl}/api/file/{fileId}");
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while deleting file!");
            }
        }

        private async Task GetFileStream(string fileLink, Stream outputStream)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(fileLink);
                var stream = await result.Content.ReadAsStreamAsync();
                await stream.CopyToAsync(outputStream);
                stream.Dispose();
            }
        }
    }
}
