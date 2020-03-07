using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Inzynierka_Core;
using Inzynierka_Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inzynierka_Core_Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void EncryptAndDecryptTest()
        {
            //Tworzymy instancje RSA w celu pozyskania kluczy
            using (var rsa = new RSACryptoServiceProvider())
            using (var encryptedStream = new MemoryStream())
            using (var decryptedStream = new MemoryStream())
            using (var memoryStream = new MemoryStream())
            {
                var expected = "Testowy ciag znakow";
                var receivers = new List<Receiver>
                {
                    new Receiver("test@o2.pl", rsa.ExportParameters(false))
                };

                using (var streamWriter = new StreamWriter(memoryStream))
                using (var streamReader = new StreamReader(decryptedStream))
                {
                    streamWriter.Write(expected);
                    streamWriter.Flush();

                    var userKeys = SafeCloudFile.Encrypt(memoryStream, encryptedStream, receivers);
                    SafeCloudFile.Decrypt(encryptedStream, decryptedStream, userKeys["test@o2.pl"],
                        rsa.ExportParameters(true));

                    decryptedStream.Position = 0;
                    var actual = streamReader.ReadToEnd();

                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod]
        public void SignAndVerifyTest()
        {
            using (var rsa = new RSACryptoServiceProvider())
            using (var encryptedStream = new MemoryStream())
            using (var memoryStream = new MemoryStream())
            {
                var expected = "Testowy ciag znakow";
                var receivers = new List<Receiver>
                {
                    new Receiver("test@o2.pl", rsa.ExportParameters(false))
                };

                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(expected);
                    streamWriter.Flush();

                    SafeCloudFile.Encrypt(memoryStream, encryptedStream, receivers);

                    var signedData = SafeCloudFile.SignFile(encryptedStream, rsa.ExportParameters(true));
                    var isValid =
                        SafeCloudFile.VerifySignedFile(encryptedStream, signedData, rsa.ExportParameters(false));

                    Assert.AreEqual(true, isValid);
                }
            }
        }
    }
}
