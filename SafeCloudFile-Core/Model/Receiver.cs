using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Inzynierka_Core.Model
{
    public class Receiver
    {
        public Receiver(string email, RSAParameters rsaKey)
        {
            Email = email;
            RsaKey = rsaKey;
        }

        public string Email { get; }
        public RSAParameters RsaKey { get;}
    }
}
