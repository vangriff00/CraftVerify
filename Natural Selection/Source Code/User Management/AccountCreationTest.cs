using System;
using System.Text;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNatural.CraftVerify.UserManager;

[TestFixture]
public class AccountCreationTests
{
    private Services instance; 

    [SetUp]
    public void Setup()
    {
        instance = new Services();
    }

    [Test]
    public void IsValidEmail_WithValidEmails_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsValidEmail("user@example.com"));
        Assert.IsTrue(instance.IsValidEmail("user.name@example.com"));
        Assert.IsTrue(instance.IsValidEmail("user-name@subdomain.example.com"));
        
    }

    [Test]
    public void IsValidEmail_WithInvalidEmails_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsValidEmail(""));
        Assert.IsFalse(instance.IsValidEmail("userexample.com"));
        Assert.IsFalse(instance.IsValidEmail("user@.com"));
        Assert.IsFalse(instance.IsValidEmail("user@exa_mple.com"));
 
    }

    [Test]
    public void IsValidDate_WithValidDates_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsValidDate("1970-01-01", 10, new DateTime(1970, 1, 1), DateTime.Now));
        Assert.IsTrue(instance.IsValidDate(DateTime.Now.ToString("yyyy-MM-dd"), 10, new DateTime(1970, 1, 1), DateTime.Now));
        
    }

    [Test]
    public void IsValidDate_WithInvalidDates_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsValidDate("1969-12-31", 10, new DateTime(1970, 1, 1), DateTime.Now));
        Assert.IsFalse(instance.IsValidDate("2025-01-01", 10, new DateTime(1970, 1, 1), DateTime.Now));
      
    }

    [Test]
    public void IsNullString_WithNullAndEmptyStrings_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsNullString(null));
        Assert.IsTrue(instance.IsNullString(""));
    }

    [Test]
    public void IsNullString_WithNonEmptyString_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsNullString("not empty"));
    }

    [Test]
    public void IsValidLength_WithValidLength_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsValidLength("12345678", 8, 64));
        Assert.IsTrue(instance.IsValidLength(new string('a', 64), 8, 64));
    }

    [Test]
    public void IsValidLength_WithInvalidLength_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsValidLength("1234567", 8, 64));
        Assert.IsFalse(instance.IsValidLength(new string('a', 65), 8, 64));
    }


    [Test]
    public void IsValidDigit_WithValidDigits_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsValidDigit("user123", @"^[a-zA-Z0-9]*$"));
        Assert.IsTrue(instance.IsValidDigit("123", @"^[0-9]*$"));
    }

    [Test]
    public void IsValidDigit_WithInvalidDigits_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsValidDigit("user@", @"^[a-zA-Z0-9]*$"));
        Assert.IsFalse(instance.IsValidDigit("abc", @"^[0-9]*$"));
    }


    [Test]
    public void IsValidPosition_WithValidPosition_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsValidPosition("user@example.com"));
    }

    [Test]
    public void IsValidPosition_WithInvalidPosition_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsValidPosition("user@"));
        Assert.IsFalse(instance.IsValidPosition("@example.com"));
        Assert.IsFalse(instance.IsValidPosition("user@@example.com"));
    }


    [Test]
    public void GenerateUserID_AlwaysGeneratesValidID()
    {
        int userId = instance.GenerateUserID();
        Assert.GreaterOrEqual(userId, 100000000); 
        Assert.LessOrEqual(userId, 999999999);
    }

    [Test]
    public void CreateEmailPepperHash_GeneratesExpectedHash()
    {
        string email = "user@example.com";
        string expectedHash = "nriewngrfewg";
        Assert.AreEqual(expectedHash, instance.CreateEmailPepperHash(email));
    }


    [Test]
    public void IsValidUserRole_WithValidRoles_ReturnsTrue()
    {
        Assert.IsTrue(instance.IsValidUserRole("admin"));
        Assert.IsTrue(instance.IsValidUserRole("regular"));
    }


    [Test]
    public void IsValidUserRole_WithInvalidRoles_ReturnsFalse()
    {
        Assert.IsFalse(instance.IsValidUserRole("admivfsn "));
        Assert.IsFalse(instance.IsValidUserRole("asn"));
        Assert.IsFalse(instance.IsValidUserRole("admsnvdvfsvf"));

    [TestFixture]
  
        private Mock<IDBInserter> _mockDbInserter;
        private AccountCreation _accountCreation;

        [SetUp]
        public void Setup()
        {
            _mockDbInserter = new Mock<IDBInserter>();

            _accountCreation = new AccountCreation(_mockDbInserter.Object);
        }

        [Test]
        public async Task InsertUserAsync_CallsInsertUserIntoTwoTablesAsync()
        {
            _mockDbInserter.Setup(x => x.InsertUserIntoTwoTablesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountCreation>()))
                           .ReturnsAsync(true);

            bool result = await _accountCreation.InsertUserAsync();

            Assert.IsTrue(result);
            _mockDbInserter.Verify(x => x.InsertUserIntoTwoTablesAsync("UserAccount", "UserProfile", _accountCreation), Times.Once);
        }

        
    

    [TearDown]
    public void Teardown()
    {
        instance = null;
    }
}