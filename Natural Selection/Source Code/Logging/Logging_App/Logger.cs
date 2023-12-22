using DataAccessLib;

namespace LoggingLib
{
    // Logger class to handle the business logic of logging messages
    public class Logger
    {
        // DataAccess object for database operations
        private readonly DataAccess _dataAccess;

        // Constructor to initialize the DataAccess object with a connection string
        public Logger(string connectionString)
        {
            _dataAccess = new DataAccess(connectionString);
        }

        // Overloaded constructor for testing that accepts a DataAccess object
        public Logger(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Asynchronous method to log a message with a given level and category
        public async Task LogAsync(string message, LogLevel level, LogCategory category)
        {

            // Check for null message
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "The log message cannot be null.");
            }

            // SemaphoreSlim to ensure that only one thread is writing to the log at a time
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
            // Wait to enter the semaphore (thread-safe operation)
            await semaphoreSlim.WaitAsync();
            try
            {
                // Save the log entry asynchronously using the DataAccess object
                await _dataAccess.SaveLogAsync(message, level, category);
            }
            finally
            {
                // Release the semaphore after the log entry is saved
                semaphoreSlim.Release();
            }
        }
    }
}
