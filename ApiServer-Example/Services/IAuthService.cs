using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;

namespace ApiServer_Example.Services
{
    public interface IAuthService
    {
        Task<AuthData> GetAuthData(Guid id);
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
    }
}
