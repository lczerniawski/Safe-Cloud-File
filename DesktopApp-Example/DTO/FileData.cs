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
        public FileData(byte[] fileSign, Dictionary<string, EncryptedAesKey> userKeys, SenderPublicKey senderPublicKey, string fileName)
        {
            FileSign = fileSign;
            UserKeys = userKeys;
            SenderPublicKey = senderPublicKey;
            FileName = fileName;
        }

        public byte[] FileSign { get; }
        public SenderPublicKey SenderPublicKey { get; }
        public Dictionary<string,EncryptedAesKey> UserKeys { get; }
        public string FileName { get; }
    }
}
