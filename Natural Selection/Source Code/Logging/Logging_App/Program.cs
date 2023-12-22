using DataAccessLib;
using LoggingLib;

namespace LoggingApp
{
    class Program
    {
        // Main entry point of the application
        static async Task Main(string[] args)
        {
            // Connection string to the database
            string connectionString = "Server=localhost;Database=LoggingDb;Trusted_Connection=True;";
            // Create an instance of the Logger with the connection string
            var logger = new Logger(connectionString);

            // Log a message asynchronously with level and category
            await logger.LogAsync("Application started", LogLevel.Info, LogCategory.Server);

            // Inform the user that the log has been created
            Console.WriteLine("Log has been created!");
        }
    }
}
