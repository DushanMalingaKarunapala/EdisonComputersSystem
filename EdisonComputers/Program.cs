using Core.Common;
using Core.Models;
using DAL;
using EdisonComputers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
namespace Presentation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                // Configure logging first
                LoggingService.ConfigureLogging();
                LoggingService.LogInformation("Application starting");

                // --- CULTURE INFO SETUP BLOCK ---
                var culture = new CultureInfo("si-LK");
                // Optionally customize the currency symbol if the default isn't exactly what you want
                // For example, if you want "LKR " instead of the default symbol "රු".
                culture.NumberFormat.CurrencySymbol = "LKR ";

                // Set the culture for the current thread
                Thread.CurrentThread.CurrentCulture = culture;
                // Set the culture for UI elements (like date pickers, etc.)
                Thread.CurrentThread.CurrentUICulture = culture;
                // --- END OF SETUP BLOCK ---


                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                //build service provider from DI container
                var serviceProvider = DIContainer.ConfigureServices();

                try
                {
                    // Create a scope to get the DbContext
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        // This command will create the database if it doesn't exist
                        // AND apply all pending migrations.
                        dbContext.Database.Migrate();

                        // You could also add your database seeding logic here if you didn't
                        // use .HasData() in OnModelCreating.

                        //use runtime seeding to create the initial admin user if it doesn't exist
                        // ✅ Safe runtime seeding - only creates if no users exist
                        if (!dbContext.Users.Any())
                        {
                            var adminUser = new User
                            {
                                Username = "admin",
                                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                                Email = "admin@edisoncomputers.lk",
                                CreatedAt = DateTime.UtcNow
                            };

                            dbContext.Users.Add(adminUser);
                            dbContext.SaveChanges();

                            MessageBox.Show(
                                "Welcome to Edison Computers Management System!\n\n" +
                                "Initial Admin Credentials:\n" +
                                "Username: admin\n" +
                                "Password: admin123\n\n" +
                                "Please change these credentials immediately after login.",
                                "Initial Setup",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }




                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"A critical error occurred while setting up the database. Please contact support.\n\nError: {ex.Message}", "Database Setup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the application if the database can't be set up.
                }



                // Resolve the CustomerService from the service provider
                var customerService = serviceProvider.GetRequiredService<BLL.Services.CustomerService.CustomerService>();

                var deviceService = serviceProvider.GetRequiredService<BLL.Services.DeviceService.DeviceService>();


                Application.Run(new Login(serviceProvider));
            }
            catch (Exception ex)
            {
                // Log the exception and show a user-friendly message
                LoggingService.LogError(ex, "An unhandled exception occurred during application startup.");
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LoggingService.LogInformation("Application shutting down");


            }
        }
    }

}