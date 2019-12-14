using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;
using AutoMapper;
using CryptoHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiServer_Example.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _jwtSecret = configuration.GetValue<string>("JWTSecretKey");
            _jwtLifespan = configuration.GetValue<int>("JWTLifespan");
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<AuthData> GetAuthData(Guid id)
        {
            if (id.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(id));

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return null;

            var expirationTime = DateTime.UtcNow.AddSeconds(_jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, id.ToString()) 
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Audience = "apiserver-example",
                Issuer = "apiserver-example"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Email = user.Email,
                RsaKeys = user.RsaKeys,
                Name = user.Name
            };
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
        }
    }
}