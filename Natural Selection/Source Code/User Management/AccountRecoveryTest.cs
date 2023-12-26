using NUnit.Framework;
using CraftVerify.NaturalSelection.UserManagment;
using System.Text.RegularExpressions;

namespace CraftVerify.NaturalSelection.UserManagment.Tests
{
    [TestFixture]
    public class AccountRecoveryServiceTests
    {
        private AccountRecoveryService _service;
        private const string ValidUserHash = "a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0a3f0";
        private const string InvalidUserHash = "12345";
        private const string ValidStatus = "Enable";
        private const string InvalidStatus = "Suspended";

        [SetUp]
        public void Setup()
        {
            // Initialize the service with valid parameters
            _service = new AccountRecoveryService(ValidUserHash, ValidStatus);
        }

        [Test]
        public void RecoverUserAccount_WithValidHashAndStatus_ReturnsTrue()
        {
            // Arrange (setup is already done)

            // Act
            bool result = _service.RecoverUserAccount(ValidUserHash, ValidStatus);

            // Assert
            Assert.IsTrue(result, "Account recovery should succeed with valid hash and status.");
        }

        [Test]
        public void RecoverUserAccount_WithInvalidHash_ReturnsFalse()
        {
            // Act
            bool result = _service.RecoverUserAccount(InvalidUserHash, ValidStatus);

            // Assert
            Assert.IsFalse(result, "Account recovery should fail with an invalid hash.");
        }

        [Test]
        public void RecoverUserAccount_WithInvalidStatus_ReturnsFalse()
        {
            // Act
            bool result = _service.RecoverUserAccount(ValidUserHash, InvalidStatus);

            // Assert
            Assert.IsFalse(result, "Account recovery should fail with an invalid status.");
        }

        [Test]
        public void IsUserHashValid_WithValidHash_ReturnsTrue()
        {
            // Act
            bool result = _service.IsUserHashValid(ValidUserHash);

            // Assert
            Assert.IsTrue(result, "The hash validation should succeed with a valid hash.");
        }

        [Test]
        public void IsUserHashValid_WithInvalidHash_ReturnsFalse()
        {
            // Act
            bool result = _service.IsUserHashValid(InvalidUserHash);

            // Assert
            Assert.IsFalse(result, "The hash validation should fail with an invalid hash.");
        }

        [Test]
        public void IsStatusValid_WithValidStatus_ReturnsTrue()
        {
            // Act
            bool result = _service.IsStatusValid(ValidStatus);

            // Assert
            Assert.IsTrue(result, "The status validation should succeed with a valid status.");
        }

        [Test]
        public void IsStatusValid_WithInvalidStatus_ReturnsFalse()
        {
            // Act
            bool result = _service.IsStatusValid(InvalidStatus);

            // Assert
            Assert.IsFalse(result, "The status validation should fail with an invalid status.");
        }

        // Add more tests for LogRecoveryRequest and any other methods as needed...
    }
}