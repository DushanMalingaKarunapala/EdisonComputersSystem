using BLL.Services.UserService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Sections.User
{
    public partial class UserSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        public UserSection(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private async void UserSection_Load(object sender, EventArgs e)
        {
            // Load user data when the UserSection is loaded
            await LoadUser();

        }

        private async Task LoadUser()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Resolve the UserService from the service provider
                    var userService = scope.ServiceProvider.GetRequiredService<BLL.Services.UserService.UserService>();
                    // Fetch user
                    var _currUser = await userService.GetUserByIdAsync(1);

                    if (_currUser != null)
                    {
                        // Display user information in the UI controls
                        txtboxusersectionUsername.Text = _currUser.Username;
                        txtboxusersectionEmail.Text = _currUser.Email;
                        //hide the password fields for security reasons

                    }
                    else
                    {
                        MessageBox.Show("User not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private async Task<bool> ValidateUserData(string newEmail)
        {



            //validate whether the emails is changed or not by checking if the textbox is empty or not
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("Email cannot be empty.", "Validation Error");
                return false;
            }


            if (!newEmail.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error");
                return false;
            }



            //validate whether the email is same as the currently stored email or not
            //get the current user email from the database
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<UserService>();
                var currentUser = await userService.GetUserByIdAsync(1); // Assuming the current user ID is 1
                if (currentUser.Email == newEmail)
                {
                    MessageBox.Show("This email is alredy saved!.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }



        private async void btnusersectionUpdate_Click(object sender, EventArgs e)
        {

            try
            {


                // 1. Get the current user ID. For a single-user app, we can assume it's 1.
                int currentUserId = 1;

                // 2. Get the new email from the textbox and validate it.
                string newEmail = txtboxusersectionEmail.Text.Trim();

                if (!await ValidateUserData(newEmail))
                {
                    return; // Exit if validation fails
                }


                using (var scope = _serviceProvider.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<UserService>();

                    // 3. Call a service method that knows how to update just the email.
                    await userService.UpdateUserProfileAsync(currentUserId, newEmail);

                    MessageBox.Show("User information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating user data: {ex.Message}", "Error");
            }

        }

        private async void btnusersectionChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                //get the user id, current password and the new password from the textboxes
                int userId = 1; // Assuming the current user ID is 1 for this example
                string currentPassword = txtboxusersectioncurrentPassword.Text;
                string newPassword = txtboxusersectionNewPassword.Text;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
                    bool success = await userService.ChangeExistingPasswordAsync(userId, newPassword, currentPassword);

                    if (success)
                    {
                        MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Password change failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (KeyNotFoundException notFoundEx)
            {
                MessageBox.Show(notFoundEx.Message, "User Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while changing the password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnusersecitonCancel_Click(object sender, EventArgs e)
        {
            // Clear the textboxes when the cancel button is clicked
            //txtboxusersectionUsername.Clear();
            txtboxusersectionEmail.Clear();
            txtboxusersectioncurrentPassword.Clear();
            txtboxusersectionNewPassword.Clear();
            MessageBox.Show("Changes have been canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Optionally, you can reload the user data to reset the UI to the original state
            LoadUser().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    MessageBox.Show($"An error occurred while reloading user data: {t.Exception?.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
