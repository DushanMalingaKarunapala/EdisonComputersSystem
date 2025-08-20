using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IUserRepository
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Finds a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User?> FindByUsernameAsync(string username);
        /// <summary>
        /// Checks if a user exists with the given username.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        Task<bool> UserExistsAsync(string username);


        //change password method
        Task<bool> ChangeExistingPasswordAsync(int userId, string newPassword, string oldPassword);

       
    }
}
