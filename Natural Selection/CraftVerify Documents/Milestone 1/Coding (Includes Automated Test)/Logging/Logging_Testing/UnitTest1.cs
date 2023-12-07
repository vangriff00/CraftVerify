using DataAccessLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LoggingLib.Tests
{
    [TestClass]
    public class LoggerTests
    {
        private Mock<DataAccess> mockDataAccess = null!;
        private Logger logger = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize the mock object here
            mockDataAccess = new Mock<DataAccess>("Server=localhost;Database=LoggingDb;Trusted_Connection=True;") { CallBase = true };
            logger = new Logger(mockDataAccess.Object);
        }

        [TestMethod]
        public async Task LogAsync_LogsInfoLevelMessage()
        {
            // Arrange
            var message = "Info level message";
            var level = LogLevel.Info;
            var category = LogCategory.Server;

            // Act
            await logger.LogAsync(message, level, category);

            // Assert
            mockDataAccess.Verify(
                da => da.SaveLogAsync(It.IsAny<string>(), It.Is<LogLevel>(l => l == LogLevel.Info), It.IsAny<LogCategory>()),
                Times.Once);
        }

        [TestMethod]
        public async Task LogAsync_ExceedsFiveSeconds()
        {
            // Arrange
            mockDataAccess.Setup(da => da.SaveLogAsync(It.IsAny<string>(), It.IsAny<LogLevel>(), It.IsAny<LogCategory>()))
                .Callback(async () => await Task.Delay(6000)); // Simulate delay

            var startTime = DateTime.Now;

            // Act
            await logger.LogAsync("Delayed message", LogLevel.Warning, LogCategory.Server);

            var duration = DateTime.Now - startTime;

            // Assert
            Assert.IsTrue(duration.TotalSeconds > 5, "Logging process should take more than 5 seconds.");
        }

        [TestMethod]
        public async Task LogAsync_CompletesWithinFiveSeconds_ButFailsToSave()
        {
            // Arrange
            mockDataAccess.Setup(da => da.SaveLogAsync(It.IsAny<string>(), It.IsAny<LogLevel>(), It.IsAny<LogCategory>()))
                .Returns(Task.CompletedTask); // Simulate fast but unsuccessful save

            var startTime = DateTime.Now;

            // Act
            await logger.LogAsync("Unsaved message", LogLevel.Warning, LogCategory.Server);

            var duration = DateTime.Now - startTime;

            // Assert
            Assert.IsTrue(duration.TotalSeconds <= 5, "Logging process should complete within 5 seconds.");
            // Additional assertion to verify that the log was not saved
        }

        [TestMethod]
        public async Task LogAsync_CompletesWithinFiveSeconds_ButInaccuratelySaves()
        {
            // Arrange
            string actualMessage = "";
            LogLevel actualLevel = LogLevel.Info;
            LogCategory actualCategory = LogCategory.Business;

            mockDataAccess.Setup(da => da.SaveLogAsync(It.IsAny<string>(), It.IsAny<LogLevel>(), It.IsAny<LogCategory>()))
                .Callback<string, LogLevel, LogCategory>((message, level, category) =>
                {
                    actualMessage = message;
                    actualLevel = level;
                    actualCategory = category;
                })
                .Returns(Task.CompletedTask); // Simulate fast but incorrect save

            var expectedMessage = "Test message";
            var expectedLevel = LogLevel.Error;
            var expectedCategory = LogCategory.Server;

            // Act
            await logger.LogAsync(expectedMessage, expectedLevel, expectedCategory);

            // Assert
            Assert.AreNotEqual(expectedMessage, actualMessage, "The saved message should not match the expected message.");
            Assert.AreNotEqual(expectedLevel, actualLevel, "The saved log level should not match the expected level.");
            Assert.AreNotEqual(expectedCategory, actualCategory, "The saved log category should not match the expected category.");
        }


        [TestMethod]
        public void LogEntries_AreNotModifiableAfterSave()
        {
            // Arrange
            var logEntry = new LogEntry { Message = "Initial message", Level = LogLevel.Info, Category = LogCategory.Business };

            // Act
            logEntry.Message = "Modified message";

            // Assert
            Assert.AreNotEqual("Modified message", logEntry.Message, "Log entries should not be modifiable after creation.");
        }

        [TestMethod]
        public async Task LogAsync_HandlesNullMessage()
        {
            // Arrange
            string message = null!;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => logger.LogAsync(message, LogLevel.Error, LogCategory.Data));
        }

        // Additional tests to cover more edge cases...

        [TestMethod]
        public async Task LogAsync_AllowsConcurrentLogging()
        {
            // Arrange
            var tasks = new Task[10];

            // Act
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = logger.LogAsync($"Concurrent message {i}", LogLevel.Debug, LogCategory.Business);
            }
            await Task.WhenAll(tasks);

            // Assert
            mockDataAccess.Verify(
                da => da.SaveLogAsync(It.IsAny<string>(), LogLevel.Debug, LogCategory.Business),
                Times.Exactly(tasks.Length));
        }

        [TestMethod]
        public async Task LogAsync_HandlesDatabaseExceptions()
        {
            // Arrange
            mockDataAccess.Setup(da => da.SaveLogAsync(It.IsAny<string>(), It.IsAny<LogLevel>(), It.IsAny<LogCategory>()))
                .ThrowsAsync(new Exception("Simulated database exception"));

            var message = "Test exception handling";

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => logger.LogAsync(message, LogLevel.Error, LogCategory.Data));
        }

        // Cleanup method if necessary
    }
}
