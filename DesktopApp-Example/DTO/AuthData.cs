using System.Security.Cryptography;

namespace DesktopApp_Example.DTO
{
    public class AuthData
    {
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
        public RSAKeys RsaKeys { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}