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
            using (var rsa = new RSACryptoServiceProvider())
            {
                var expected = "Testowy ciag znakow";
                var receivers = new List<Receiver>
                {
                    new Receiver("test", "test@o2.pl", rsa.ExportParameters(false))
                };

                var memoryStream = new MemoryStream();
                var streamWriter = new StreamWriter(memoryStream);
                streamWriter.Write(expected);
                streamWriter.Flush();

                var encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);
                var decryptedStream = SafeCloudFile.Decrypt(encryptedFile.EncryptedStream,
                    encryptedFile.UserKeys["test@o2.pl"], rsa.ExportParameters(true));

                var streamReader = new StreamReader(decryptedStream);

                var actual = streamReader.ReadToEnd();

                memoryStream.Dispose();
                streamWriter.Dispose();
                encryptedFile.EncryptedStream.Dispose();
                decryptedStream.Dispose();
                streamReader.Dispose();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void SignAndVerifyTest()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var expected = "Testowy ciag znakow";
                var receivers = new List<Receiver>
                {
                    new Receiver("test", "test@o2.pl", rsa.ExportParameters(false))
                };

                var memoryStream = new MemoryStream();
                var streamWriter = new StreamWriter(memoryStream);
                streamWriter.Write(expected);
                streamWriter.Flush();

                var encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);

                var signedData = SafeCloudFile.SignFile(encryptedFile.EncryptedStream,rsa.ExportParameters(true));
                var isValid = SafeCloudFile.VerifySignedFile(encryptedFile.EncryptedStream, signedData,rsa.ExportParameters(false));

                memoryStream.Dispose();
                streamWriter.Dispose();
                encryptedFile.EncryptedStream.Dispose();

                Assert.AreEqual(true,isValid);
            }
        }
    }
}
