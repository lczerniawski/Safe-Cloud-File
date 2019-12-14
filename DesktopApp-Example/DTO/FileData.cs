using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inzynierka_Core.Model;

namespace DesktopApp_Example.DTO
{
    public class FileData
    {
        public FileData(byte[] fileSign, Dictionary<string, EncryptedAesKey> userKeys)
        {
            FileSign = fileSign;
            UserKeys = userKeys;
        }

        public byte[] FileSign { get; }
        public Dictionary<string,EncryptedAesKey> UserKeys { get; }
    }
}
