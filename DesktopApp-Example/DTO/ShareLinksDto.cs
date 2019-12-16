using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp_Example.DTO
{
    public class ShareLinksDto
    {
        public ShareLinksDto(string encryptedFileLink, string jsonFileLink)
        {
            EncryptedFileLink = encryptedFileLink;
            JsonFileLink = jsonFileLink;
        }

        public string EncryptedFileLink { get; }
        public string JsonFileLink { get; }
    }
}
