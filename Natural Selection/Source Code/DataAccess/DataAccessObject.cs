using System.Dynamic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System;

namespace MyLibrary
{
    public class Result
    {
        public bool hasError { get; set; }
        public string? errorMessage { get; set; }
        public string? statusCode { get; set; }
    }

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

    public class DataAccessObject
    {

        // Connection string to connect to the database
        private readonly string _connectionString;

        // Constructor to set the database connection string
        public DataAccessObject(string connectionString)
        {
            _connectionString = connectionString;
        }
        //Checks Username to check if it follows username requirements
        private bool isValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) && username.Length >= 6 && username.Length <= 30 && !username.Contains(" ");
        }
        //Checks Email to check if it follows Email requirements
        private bool isValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return !string.IsNullOrWhiteSpace(email) && System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }
        //Checks DOB to check if it follows DOB requirements
        private bool isValidDateOfBirth(DateTime dateOfBirth)
        {
            DateTime minDate = new DateTime(1970, 1, 1);
            return dateOfBirth >= minDate && dateOfBirth <= DateTime.Today;
        }
        //Checks Password to check if it follows Password requirements
        private bool isValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8
                && containsLowercase(password) && containsUppercase(password) && containsSingleDigit(password);
        }
        //Checks to make sure a string contains lowercase letters
        private bool containsLowercase(string password)
        {
            return password.Any(char.IsLower);
        }
        //Checks to make sure a string contains uppercase letters
        private bool containsUppercase(string password)
        {
            return password.Any(char.IsUpper);
        }
        //Checks to make sure a string contains a single digit
        private bool containsSingleDigit(string password)
        {
            return password.Count(char.IsDigit) == 1;
        }

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

        public Result createUser(string firstName, string lastName, string username, string email, DateTime dateOfBirth, string password)
        {
            var result = new Result();

            if (!isValidUsername(username))
            {
                result.hasError = true;
                result.errorMessage = "Invalid username. Username must be between 6 and 30 characters, and cannot contain spaces.";
                return result;
            }

            if (!isValidEmail(email))
            {
                result.hasError = true;
                result.errorMessage = "Invalid email format.";
                return result;
            }

            if (!isValidDateOfBirth(dateOfBirth))
            {
                result.hasError = true;
                result.errorMessage = "Invalid date of birth. It should be between January 1st, 1970, and the current date.";
                return result;
            }

            if (!isValidPassword(password))
            {
                result.hasError = true;
                result.errorMessage = "Invalid password format. Must be more than 8 characters and contain uppercase and lowercase letters as well as a single digit.";
                return result;
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var sql = "INSERT INTO users (firstName, lastName, username, email, dateOfBirth, password) " +
                            "VALUES (@FirstName, @LastName, @Username, @Email, @DateOfBirth, @Password)";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@Password", password);
                        command.ExecuteNonQuery();
                    }
                }
                result.hasError = false;
            }
            catch (Exception ex)
            {
                result.hasError = true;
                result.errorMessage = ex.Message;
            }

            return result;
        }


        public int Users { get; private set; } // Updated to a private set
    }
/*
    public class Logger
    {
        private DataAccessObject dao; // Create DataAccessObject once for Logger

        public Logger()
        {
            dao = new DataAccessObject();
        }

        public Result log(string logLevel, string logCategory, string context)
        {
            return dao.createLog(DateTime.UtcNow, logLevel, logCategory, context);
        }
    }
*/

    /*Parth's Code
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



    //Prof. Lecture 11/8/2023
    public interface IReadOnlyDAO
    {
        String connString = “Server;Database;UID=DbUserReadOnly;Password”	
        Response Query(string ExecuteSql)

        Response Query(string sql, int count = int.MaxValue, int currentPage = 0)//Pagination
    }

    public interface IWriteDAO
    {
        Response WriteData(string sql)
    }

    Public class DataGateway: IReadOnlyDAO

    

    //AdvanceSqlDAO.cs
    public class AdvanceSqlDAO : ISqlDAO
    {
        public Response ExecuteSql(string sql){

        }
    }

    */

    /*
        Response <T>
            bool hasError = True
            String? errorMessage = null
            ICollection <objects = null
            int attempts = 0
            bool safeToRetry = False
    */
}
