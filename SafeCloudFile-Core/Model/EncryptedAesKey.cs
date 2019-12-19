using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka_Core.Model
{
    public class EncryptedAesKey
    {
        public EncryptedAesKey(byte[] encryptedKey,byte[] encryptedIv)
        {
            EncryptedKey = encryptedKey;
            EncryptedIV = encryptedIv;
        }

        public byte[] EncryptedKey { get; }
        public byte[] EncryptedIV { get; }
    }
}
