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

                aes.GenerateKey();
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                var encryptedStream = new MemoryStream();
                CryptoTransformStream(plainStream,encryptedStream,encryptor);
                
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

                using (Aes aes = Aes.Create())
                {
                    if (aes == null) throw new Exception("Error while initializing AES instance.");

                    aes.Key = decryptedKey;
                    aes.IV = decryptedIv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    var decryptedStream = new MemoryStream();
                    using (var innerEncryptedStream = new MemoryStream(encryptedStream.ToArray()))
                    {
                        CryptoTransformStream(innerEncryptedStream, decryptedStream, decryptor);
                    }

                    return new MemoryStream(decryptedStream.ToArray());
                }
            }
        }

        public static byte[] SignFile(MemoryStream memoryStreamToSign,RSAParameters senderKey)
        {
            if (memoryStreamToSign == null) throw new ArgumentNullException(nameof(memoryStreamToSign));

            memoryStreamToSign.Position = 0;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(senderKey);

                return rsa.SignData(memoryStreamToSign,HashAlgorithmName.SHA256,RSASignaturePadding.Pkcs1);
            }
        }

        public static bool VerifySignedFile(MemoryStream memoryStreamToVerify,byte[] signedBytes, RSAParameters senderKey)
        {
            if (memoryStreamToVerify == null) throw new ArgumentNullException(nameof(memoryStreamToVerify));
            if (signedBytes == null) throw new ArgumentNullException(nameof(signedBytes));

            memoryStreamToVerify.Position = 0;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(senderKey);

                return rsa.VerifyData(memoryStreamToVerify, signedBytes,HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        private static void CryptoTransformStream(MemoryStream inStream,MemoryStream outStream, ICryptoTransform cryptoTransform)
        {
            using (CryptoStream decrypt = new CryptoStream(inStream, cryptoTransform, CryptoStreamMode.Read))
            {
                using (var binaryWriter = new BinaryWriter(outStream))
                {
                    byte[] buffer = new byte[10485760]; // read in chunks of 10MB
                    int bytesRead;
                    while ((bytesRead = decrypt.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        binaryWriter.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}
