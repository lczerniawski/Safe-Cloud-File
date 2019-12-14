using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiServer_Example.Data;
using ApiServer_Example.Domains.Models;
using Microsoft.EntityFrameworkCore;
using Guid = System.Guid;

namespace ApiServer_Example.Services
{

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> IsEmailUniqAsync(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user == null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var result = _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            if(Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            var result = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (result == null)
                return false;

            _context.Users.Remove(result);

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            if(Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var userInDb = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
            if (userInDb == null)
                return false;

            var updatedEntity = _context.Users.Update(user);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
