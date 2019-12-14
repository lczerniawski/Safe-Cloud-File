using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;

namespace ApiServer_Example.Services
{
    public interface IUserRepository
    {
        Task<bool> IsEmailUniqAsync(string email);
        Task<User> GetUserByEmailAsync(string email);

        Task<User> CreateUserAsync(User user);

        Task<bool> DeleteUserAsync(Guid id);

        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserByIdAsync(Guid id);

        Task<bool> UpdateUserAsync(User user);
    }
}
