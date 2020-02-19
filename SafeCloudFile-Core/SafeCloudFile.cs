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
        public static Dictionary<string, EncryptedAesKey> Encrypt(Stream inputStream,Stream outputStream, List<Receiver> receiversList)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (receiversList == null) throw new ArgumentNullException(nameof(receiversList));
            inputStream.Position = 0;

            using (Aes aes = Aes.Create())
            {
                if (aes == null) throw new Exception("Error while initializing AES instance.");

                aes.GenerateKey();
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                CryptoTransformStream(inputStream,outputStream,encryptor);
                
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

                return encryptedKeys;
            }
        }

        public static void Decrypt(Stream inputStream,Stream outputStream, EncryptedAesKey encryptedAesKey, RSAParameters receiverKey)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
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
                    inputStream.Position = 0;
                    CryptoTransformStream(inputStream, outputStream, decryptor);
                }
            }
        }

        public static byte[] SignFile(Stream memoryStreamToSign,RSAParameters senderKey)
        {
            if (memoryStreamToSign == null) throw new ArgumentNullException(nameof(memoryStreamToSign));

            memoryStreamToSign.Position = 0;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(senderKey);

                return rsa.SignData(memoryStreamToSign,HashAlgorithmName.SHA256,RSASignaturePadding.Pkcs1);
            }
        }

        public static bool VerifySignedFile(Stream memoryStreamToVerify,byte[] signedBytes, RSAParameters senderKey)
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

        private static void CryptoTransformStream(Stream inStream,Stream outStream, ICryptoTransform cryptoTransform)
        {
            using (CryptoStream decrypt = new CryptoStream(inStream, cryptoTransform, CryptoStreamMode.Read))
            {
                using (var binaryWriter = new BinaryWriter(outStream,Encoding.UTF8,true))
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
