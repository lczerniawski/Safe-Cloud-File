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
    public interface IFileService
    {
        Task<ShareLinksDto> UploadFile(string fileName, string fileExtension, FileStream fileStream, List<Receiver> receivers, RSAParameters senderKey,bool isShared);
        Task<List<ViewFile>> GetAllFiles();
        Task<MemoryStream> DownloadFile(string path,ViewFile file,string receiverEmail,RSAParameters receiverKey);
        Task<SharedDownload> DownloadShared(string encryptedFileLink, string jsonFileLink,string receiverEmail, RSAParameters receiverKey);
        Task DeleteFile(ViewFile file);
    }
}
