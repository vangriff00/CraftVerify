using System.Data.SqlClient;

namespace DataAccessLib
{
    // LogEntry class to represent a log record
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Initialize with current UTC time
        public LogLevel Level { get; set; } = LogLevel.Info; // Default to Info level
        public LogCategory Category { get; set; } = LogCategory.Business; // Default to some category
        public string Message { get; set; } = string.Empty; // Initialize as an empty string
    }


    // Enum to define log levels
    public enum LogLevel
    {
        Info,
        Debug,
        Warning,
        Error
    }

    // Enum to define log categories
    public enum LogCategory
    {
        View,
        Business,
        Server,
        Data,
        DataStore
    }

    // DataAccess class to handle database operations for logging
    public class DataAccess
    {
        // Connection string to connect to the database
        private readonly string _connectionString;

        // Constructor to set the database connection string
        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Asynchronous method to save a log entry to the database
        public virtual async Task SaveLogAsync(string message, LogLevel level, LogCategory category)
        {
            // Create a log entry object with the current UTC time, log level, category, and message
            var logEntry = new LogEntry
            {
                Message = message,
                Level = level,
                Category = category,
                Timestamp = DateTime.UtcNow
            };

            // Open a new SQL connection using the connection string
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // SQL command to insert the log entry into the Logs table
                var commandText = "INSERT INTO Logs (Message, Level, Category, Timestamp) VALUES (@Message, @Level, @Category, @Timestamp)";
                using (var command = new SqlCommand(commandText, connection))
                {
                    // Add parameters to the SQL command to prevent SQL injection
                    command.Parameters.AddWithValue("@Message", logEntry.Message);
                    command.Parameters.AddWithValue("@Level", logEntry.Level.ToString()); // Convert enum to string
                    command.Parameters.AddWithValue("@Category", logEntry.Category.ToString()); // Convert enum to string
                    command.Parameters.AddWithValue("@Timestamp", logEntry.Timestamp);
                    // Execute the command asynchronously
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}