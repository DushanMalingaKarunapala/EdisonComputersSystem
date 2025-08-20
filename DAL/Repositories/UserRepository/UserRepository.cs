using BCrypt.Net;
using Core.IRepository.IUserRepository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        private readonly ApplicationDbContext _context;


        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ChangeExistingPasswordAsync(int userId, string newPassword, string oldPassword)
        {
            // Use FindAsync for asynchronous operation
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                // Don't throw. Just return false. You don't want to tell an attacker
                // if a user ID exists or not.
                return false;
            }

            // Check if the old password matches
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            {
                // Password doesn't match, return false.
                return false;
            }

            // Hash the NEW password and update the user object
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Save the changes asynchronously
            await _context.SaveChangesAsync();

            // Return true indicating success
            return true;




        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() ==username.ToLower());
        }


        public Task<bool> UserExistsAsync(string username)
        {
            throw new NotImplementedException();
        }

        

        //public async Task<User?> GetUserByIdAsync(int userId)
        //{
        //    return await _context.Users.FindAsync(userId);
        //}
    }
}
