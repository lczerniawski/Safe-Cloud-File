using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Inzynierka_Core.Model;

namespace Inzynierka_Core
{
    public static class SafeCloudFile
    {
        public static EncryptedFile Encrypt(MemoryStream plainStream, List<Receiver> receiversList)
        {
            if (plainStream == null) throw new ArgumentNullException(nameof(plainStream));
            if (receiversList == null) throw new ArgumentNullException(nameof(receiversList));
            plainStream.Position = 0;

            using (Aes aes = Aes.Create())
            {
                if (aes == null) throw new Exception("Error while initializing AES instance.");

                //Todo Alternatywa do wbudowanego generania hasla
                //Rfc2898DeriveBytes aesKey = new Rfc2898DeriveBytes("dsadsad",15);
                //myAlg.EncryptedKey = key.GetBytes( myAlg.KeySize / 8);
                //myAlg.EncryptedIV  = key.GetBytes( myAlg.BlockSize / 8);

                aes.GenerateKey();
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                var encryptedStream = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                    {
                        byte[] buffer = new byte[2048]; // read in chunks of 2KB
                        int bytesRead;
                        while ((bytesRead = plainStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            swEncrypt.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                
                var encryptedKeys = new Dictionary<string, EncryptedAesKey>();
                foreach (var receiver in receiversList)
                {
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(receiver.RsaKey);

                        var encryptedKey = rsa.Encrypt(aes.Key, false);
                        var encryptedIv = rsa.Encrypt(aes.IV, false);

                        var userAesKey = new EncryptedAesKey(encryptedKey, encryptedIv);

                        encryptedKeys.Add(receiver.Email, userAesKey);
                    }
                }

                return new EncryptedFile(new MemoryStream(encryptedStream.ToArray()), encryptedKeys);
            }
        }

        public static MemoryStream Decrypt(MemoryStream encryptedStream,EncryptedAesKey encryptedAesKey, RSAParameters receiverKey)
        {
            if (encryptedStream == null) throw new ArgumentNullException(nameof(encryptedStream));
            if (encryptedAesKey == null) throw new ArgumentNullException(nameof(encryptedAesKey));

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(receiverKey);
                var decryptedKey = rsa.Decrypt(encryptedAesKey.EncryptedKey,false);
                var decryptedIv = rsa.Decrypt(encryptedAesKey.EncryptedIV, false);
                var innerEncryptedStream = new MemoryStream(encryptedStream.ToArray());

                using (Aes aes = Aes.Create())
                {
                    if (aes == null) throw new Exception("Error while initializing AES instance.");

                    aes.Key = decryptedKey;
                    aes.IV = decryptedIv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    var decryptedStream = new MemoryStream();
                    using (CryptoStream csDecrypt =
                        new CryptoStream(innerEncryptedStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var binaryWriter = new BinaryWriter(decryptedStream))
                        {
                            byte[] buffer = new byte[2048]; // read in chunks of 2KB
                            int bytesRead;
                            while ((bytesRead = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                binaryWriter.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    return new MemoryStream(decryptedStream.ToArray());
                }
            }
        }
    }
}
