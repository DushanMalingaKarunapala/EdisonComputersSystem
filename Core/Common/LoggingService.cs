using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class LoggingService
    {
        public static void ConfigureLogging()
        {
            // Create logs directory in application's base directory
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory); // Ensure directory exists
                                                     // Configure Serilog to write logs to a file in the logs directory
            var logPath = Path.Combine(logDirectory, "edison-computers-.txt");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30)
              
                .CreateLogger();
        }

        public static void LogError(Exception ex, string message)
        {
            Log.Error(ex, message);
        }

        public static void LogInformation(string message)
        {
            Log.Information(message);
        }

        public static void LogWarning(string message)
        {
            Log.Warning(message);
        }
    }
}
