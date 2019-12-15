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
using Google.Apis.Upload;
using Inzynierka_Core;
using Inzynierka_Core.Model;
using Newtonsoft.Json;

namespace DesktopApp_Example.Services
{
    public class OwnServerFileService : IFileService
    {
        private readonly AuthData _authData;

        public OwnServerFileService(AuthData authData)
        {
            _authData = authData;
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
            if (uploadJsonFileDto == null)
                throw new Exception("Error while uploading file!");

            var uploadFileStatus = await UploadFile(fileName, fileExtension, encryptedFile.EncryptedStream,uploadJsonFileDto.Id);
            if(uploadFileStatus == null)
                throw new Exception("Error while uploading file!");
        }

        public async Task<List<ViewFile>> GetAllFiles()
        {
            List<ViewFile> result = new List<ViewFile>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);
                var response = await client.GetAsync($"https://localhost:44312/api/file");
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

        public Task DownloadFile(string path, ViewFile file, string receiverEmail, RSAParameters receiverKey, RSAParameters senderKey)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteFile(ViewFile file)
        {
            await DeleteFile(file.JsonFileId);
            await DeleteFile(file.Id);
        }

        private async Task<FileDto> UploadFile(string fileName,string fileExtension,Stream stream,string jsonFileId)
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
                var result = await client.PostAsync("https://localhost:44312/api/file", content);
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while uploading file!");

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<FileDto>(resultString);
            }
        }

        private async Task<FileDto> UploadJson(string fileName,Stream stream)
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
                var result = await client.PostAsync("https://localhost:44312/api/file", content);
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while uploading file!");

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<FileDto>(resultString);
            }
        }

        private async Task DeleteFile(string fileId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authData.Token);

                var result = await client.DeleteAsync($"https://localhost:44312/api/file/{fileId}");
                if(!result.IsSuccessStatusCode)
                    throw new Exception("Error while deleting file!");
            }
        }
    }
}
