# Safe-Cloud-File

>Library for secure storing and sharing files in a cloud using hybrid cryptography.

## Description

Library that provides easy to use API for encrypting, decrypting and signing files using hybrid cryptography (RSA + AES).
Repository provides example desktop application with this library as dependency, to show how it can be potentially used to provide safe file storage in cloud.
Example application use file server implemented by author of repository alongside Google Drive and Microsoft OneDrive provider.
File server is also responsible for authenticating users and distribute cryptographic keys.
Repository is part of author diploma work.

**Application Showcase**

![Application Showcase](http://g.recordit.co/YfzWs24djo.gif)

## How To Use
Library provides four self descriptive methods:

 ```csharp
public static Dictionary<string, EncryptedAesKey> Encrypt(Stream inputStream, 
                                                          Stream outputStream, 
                                                          List<Receiver> receiversList)
```

Allows encrypting file, it accepts input stream, output stream, and list of file receivers. Receivers list allows to provide many file recipients,
and share file to others users. Single receiver consists of email address and RSA public key. For every receiver new AES key is created. 
Method returns dictionary which contains receiver email as key, and encrypted AES key needed to decrypt file by recipient.

```csharp
public static void Decrypt(Stream inputStream, Stream outputStream, 
                           EncryptedAesKey encryptedAesKey, RSAParameters receiverKey)
```
Used to decrypt file, it accepts input stream, output stream, encrypted aes key which is provided alongside file encrypting, and receiver rsa key.

```csharp
public static byte[] SignFile(Stream streamToSign, RSAParameters senderKey)
```
Sign file with provided rsa key and returns digital signature as byte array.

```csharp
public static bool VerifySignedFile(Stream streamToVerify, byte[] signedBytes, RSAParameters senderKey)
```

Verify signature, returns boolean value indicating is signature is valid.
