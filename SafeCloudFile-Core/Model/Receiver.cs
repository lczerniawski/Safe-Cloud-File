using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Inzynierka_Core.Model
{
    public class Receiver
    {
        public Receiver(string name, string email, RSAParameters rsaKey)
        {
            Name = name;
            Email = email;
            RsaKey = rsaKey;
        }

        public string Name { get; }
        public string Email { get; }
        public RSAParameters RsaKey { get;}
    }
}
