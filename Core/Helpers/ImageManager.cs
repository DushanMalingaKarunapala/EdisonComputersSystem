using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    /// <summary>
    /// a helper class for managing saving and loading images in the application
    /// </summary>
    public class ImageManager
    {
        //base images directory in the application data folder
        //private static readonly string BaseImagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeviceImages");

        // ✅ FIXED: Use persistent location that survives app updates. even the application is deleted, the images should remain.
        private static readonly string BaseImagesPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EdisonComputers",
            "DeviceImages"
        );
        // This creates: C:\Users\[User]\AppData\Roaming\EdisonComputers\DeviceImages


        //a method to get the device images folder path based on the argument deviceId
        public static string GetDeviceImagesFolder(int deviceId)
        {
            var deviceFolder = Path.Combine(BaseImagesPath, $"Device_{deviceId}");
            Directory.CreateDirectory(deviceFolder); // Create if doesn't exist
            return deviceFolder;
        }

        //a method to save the device image and return the relative path for database storage
        public static string SaveDeviceImage(int deviceId, string sourceImagePath)
        {
            try
            {
                var deviceFolder = GetDeviceImagesFolder(deviceId);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(sourceImagePath)}";
                var destinationPath = Path.Combine(deviceFolder, fileName);

                // Copy the image to our managed folder
                File.Copy(sourceImagePath, destinationPath, overwrite: true);

                // Return relative path for database storage
                return Path.Combine($"Device_{deviceId}", fileName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save device image: {ex.Message}", ex);
            }
        }

        //a method to get the full image path based on the relative path stored in the database
        public static string GetFullImagePath(string relativePath)
        {
            return Path.Combine(BaseImagesPath, relativePath);
        }

        //a method to check if the image exists based on the relative path stored in the database
        public static bool ImageExists(string relativePath)
        {
            var fullPath = GetFullImagePath(relativePath);
            return File.Exists(fullPath);
        }

        //a method to delete the image based on the relative path stored in the database
        public static void DeleteDeviceImage(string relativePath)
        {
            try
            {
                var fullPath = GetFullImagePath(relativePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                // Log but don't throw - image cleanup shouldn't break the app
                Console.WriteLine($"Failed to delete image {relativePath}: {ex.Message}");
            }
        }

        //a method to delete the entire device folder and all its images based on the deviceId
        public static void DeleteDeviceFolder(int deviceId)
        {
            try
            {
                var deviceFolder = Path.Combine(BaseImagesPath, $"Device_{deviceId}");
                if (Directory.Exists(deviceFolder))
                {
                    Directory.Delete(deviceFolder, recursive: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete device folder {deviceId}: {ex.Message}");
            }
        }

    }
}
