using Core.IRepository;
using Core.IRepository.IUserRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        //private readonly IRepository<User> _repository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // This method will check the username and then verify the hashed password
        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            // Find the user by username
            var user = await _userRepository.FindByUsernameAsync(username);
            if (user == null)
            {
                return false; // User not found
            }
            // Verify the provided password against the stored hash
            // We will use a library for this.
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }


        /// <summary>
        /// add user method///
        /// 
        public async Task AddUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            // Check if the user already exists
            var existingUser = await _userRepository.FindByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with username {user.Username} already exists.");
            }
            // Hash the password before saving
            var newuser = new User
            {
                Username = user.Username,
                CreatedAt = DateTime.Now,
                Email = user.Email,

                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash) // Hash the password
            };
            await _userRepository.Add(user);
        }


        //get user by id method
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }
            return await _userRepository.GetById(userId);
        }


        //update user method
        public async Task UpdateUserProfileAsync(int userId, string newEmail)
        {
            // The service layer is a good place for this logic.
            // It fetches the user, modifies it, and tells the repo to save.
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Modify ONLY the properties that should be changed by this action.
            user.Email = newEmail;
            // user.Username is NOT changed.
            // user.PasswordHash is NOT changed.

            await _userRepository.Update(user); // Call the generic Update method
        }



        //change password method
        public async Task<bool> ChangeExistingPasswordAsync(int userId, string newPassword, string oldPassword)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(oldPassword))
            {
                throw new ArgumentException("New password and old password cannot be empty.");
            }
            //check whether the old password is correct
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            {
                throw new ArgumentException("Old password is incorrect.");
            }

            else if (newPassword.Length < 8)
            {
                throw new ArgumentException("New password must be at least 8 characters long.");
            }
            return await _userRepository.ChangeExistingPasswordAsync(userId, newPassword, oldPassword);
        }



    }
}
