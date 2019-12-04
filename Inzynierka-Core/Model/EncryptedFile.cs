using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka_Core.Model
{
    public class EncryptedFile
    {
        public EncryptedFile(MemoryStream encryptedStream, Dictionary<string, EncryptedAesKey> userKeys)
        {
            EncryptedStream = encryptedStream;
            UserKeys = userKeys;
        }

        public MemoryStream EncryptedStream { get; }
        public Dictionary<string,EncryptedAesKey> UserKeys { get; }
    }
}
