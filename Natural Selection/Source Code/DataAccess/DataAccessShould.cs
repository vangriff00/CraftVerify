namespace MyLibraryTest;

using System.Diagnostics;

//TDD- Test Driven Development

//Unit Testing - You Define a unit of work and test that

/*
Smoke test
Regression Testing
Integration Testing
penetration testing
End 1

*/
using MyLibrary;



public class DataAccessShould
{
    String testConnectionString = "";

    [Fact]
    //Example method name: Classname_MethodName_Scenario()
    public void createNewUserInDataStore()
    {
        //Triple A

        //Arrange
        var timer = new Stopwatch();
        var dao = new DataAccessObject(testConnectionString);
        //var insertSql = "INSERT INTO users Values('John', 'Smith')";
        
        //Create's User Date of Birth
        var userDateOfBirth = new DateTime(1980, 5, 1, 9, 15, 0);

        //Act
        timer.Start();
        var result = dao.createUser("John", "Smith", "JohnSmith1", "JohnSmith@gmail.com",userDateOfBirth, "Password1");
        timer.Stop();

        //Assert
        Assert.True(result.hasError == false);
        Assert.True(timer.Elapsed.TotalSeconds <= 3);
        //Assert that data record exists in data store and saved accurately
    }

    [Fact]
    public void createUser_With_EmptyName_First_And_Last_Names()
    {
        // Test creating a user with empty first and last names

        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        //Create's User Date of Birth
        var userDateOfBirth = new DateTime(1982, 7, 4, 10, 30, 0);

        // Act
        var result = dao.createUser("", "", "JohnSmith1", "JohnSmith@gmail.com", userDateOfBirth, "Password1");

        // Assert
        Assert.True(result.hasError); // Expect an error for empty names
        Assert.NotNull(result.errorMessage); // Error message should not be null
    }

    [Fact]
    public void createUser_With_EmptyName_FirstName_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid first name", result.errorMessage);
    }

    [Fact]
    public void createUser_With_EmptyName_LastName_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid last name", result.errorMessage);
    }



    [Fact]
    public void createUser_InvalidUsername_EmptyUsername_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "", "john@example.com", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid username", result.errorMessage);
    }

    
    [Fact]
    public void createUser_InvalidUsername_TooShort_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "a", "john@example.com", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid username", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidUsername_TooLong_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "ThisUsernameIsTooLongAndExceedsMaximumAllowedLength", "john@example.com", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid username", result.errorMessage);
    }


    [Fact]
    public void createUser_InvalidUsername_ContainsSpace_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "John Space", "john@example.com", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid username", result.errorMessage);
    }


    [Fact]
    public void createUser_InvalidEmail_NoAddress_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "john_smith", "invalid_email", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid email format", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidEmail_EmptyEmail_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "", new DateTime(1990, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid email format", result.errorMessage);
    }


    [Fact]
    public void createUser_InvalidDateOfBirth_TooEarly_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "john_smith", "john@example.com", new DateTime(1800, 1, 1), "Password1");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid date of birth", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidPassword_EmptyPassword_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "");

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid password format", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidPassword_TooShort_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "Pwd1"); // Assuming "Pwd1" is too short

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid password format", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidPassword_TooLong_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "LongPasswordWithM0reThanAllowedCharacters"); // Assuming the password is too long

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid password format", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidPassword_ContainsNoNumbers_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "NoNumbersInPassword"); // Assuming the password contains no numbers

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid password format", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidPassword_ContainsNoUppercaseLetters_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "onlylowercase1"); // Assuming the password contains no uppercase letters

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid password format", result.errorMessage);
    }

    [Fact]
    public void createUser_InvalidPassword_ContainsNoLowercaseLetters_ThrowsError()
    {
        // Arrange
        var dao = new DataAccessObject(testConnectionString);

        // Act
        var result = dao.createUser("John", "Smith", "JohnSmith", "john@example.com", new DateTime(1990, 1, 1), "ONLYUPPERCASE1"); // Assuming the password contains no lowercase letters

        // Assert
        Assert.True(result.hasError);
        Assert.Contains("Invalid password format", result.errorMessage);
    }
}

/*
public class LoggerTests
{
    [Fact]
    public void log_NullArgumentPassedIn_ThrowsArgumentNullException()
    {
        // Test logging with a null log context argument

        // Arrange
        var logger = new Logger();
        string? logContext = null;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {   
            if (logContext == null)
            {
                throw new ArgumentNullException(nameof(logContext));
            }

            logger.log("Level", "Category", logContext);
        });
    }
    
    [Fact]
    public void Log_ValidLogInfo_ReturnsNoError()
    {
        // Arrange
        var logger = new Logger();

        // Act
        var result = logger.log("Info", "TestCategory", "Log information");

        // Assert
        Assert.False(result.hasError);
        Assert.Null(result.errorMessage);
    }

    [Fact]
    public void Log_NullLogLevel_ThrowsArgumentNullException()
    {
        // Arrange
        var logger = new Logger();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => logger.log(null!, "TestCategory", "Log information"));
    }

    [Fact]
    public void logWithoutError()
    {
        // Test logging without any error

        // Arrange
        var logger = new Logger();

        // Act
        var result = logger.log("Info", "General", "Log message without error");

        // Assert
        Assert.False(result.hasError); // Expect no error during logging
    }
}
*/