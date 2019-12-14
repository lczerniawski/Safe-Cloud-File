using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;
using Inzynierka_Core.Model;

namespace DesktopApp_Example.Services
{
    public class OneDriveFileService : IFileService
    {
        public Task UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewFile>> GetAllFiles()
        {
            throw new NotImplementedException();
        }

        public Task DownloadFile(string path, ViewFile file, string receiverEmail, RSAParameters receiverKey, RSAParameters senderKey)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(ViewFile file)
        {
            throw new NotImplementedException();
        }
    }
}
