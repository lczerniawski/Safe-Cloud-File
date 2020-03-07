using System.Security.Cryptography;
using ApiServer_Example.Domains.Models;

namespace ApiServer_Example.Domains.DTO
{
    public class AuthData
    {
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
        public RSAKeys RsaKeys { get; set; }
        public string Email { get; set; }
    }
}