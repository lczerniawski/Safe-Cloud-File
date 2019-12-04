using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Inzynierka_Core;
using Inzynierka_Core.Model;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            EncryptedFile encryptedFile;
            using (var rsa = new RSACryptoServiceProvider())
            {
                using (var fileStream = new FileStream("test.txt", FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var receivers = new List<Receiver>
                        {
                            new Receiver("test","test@o2.pl",rsa.ExportParameters(false))
                        };
                        fileStream.CopyTo(memoryStream);
                        encryptedFile = SafeCloudFile.Encrypt(memoryStream, receivers);
                    }
                }

                using (var streamReader = new StreamReader(new MemoryStream(encryptedFile.EncryptedStream.ToArray())))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }

                var signedData = SafeCloudFile.SignFile(encryptedFile.EncryptedStream,rsa.ExportParameters(true));
                var isValid = SafeCloudFile.VerifySignedFile(encryptedFile.EncryptedStream, signedData,
                    rsa.ExportParameters(false));

                Console.WriteLine(isValid);

                var decryptedStream = SafeCloudFile.Decrypt(encryptedFile.EncryptedStream, encryptedFile.UserKeys["test@o2.pl"], rsa.ExportParameters(true));
                using (var streamReader = new StreamReader(decryptedStream))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }


                Console.ReadKey();
            }
        }
    }
}
