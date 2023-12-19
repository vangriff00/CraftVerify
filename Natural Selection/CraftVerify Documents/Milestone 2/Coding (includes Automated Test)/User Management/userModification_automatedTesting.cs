using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

[TestClass]
public class UserModificationTests
{
    [TestMethod]
    public void UpdateUserProfile_ValidBioAndPhoto_ReturnsTrue()
    {
        // Arrange
        var userMod = new UserModification();
        string validBio = "This is a valid bio.";
        byte[] validPhoto = new byte[] { 0xFF, 0xD8 }; // Mock JPEG header

        // Act
        bool result = userMod.UpdateUserProfile("testUserId", validBio, validPhoto);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void UpdateUserProfile_InvalidBio_ReturnsFalse()
    {
        // Arrange
        var userMod = new UserModification();
        string invalidBio = new string('x', 1001); // Mock invalid bio
        byte[] validPhoto = new byte[] { 0xFF, 0xD8 }; // Mock JPEG header

        // Act
        bool result = userMod.UpdateUserProfile("testUserId", invalidBio, validPhoto);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void UpdateUserProfile_InvalidPhoto_ReturnsFalse()
    {
        // Arrange
        var userMod = new UserModification();
        string validBio = "This is a valid bio.";
        byte[] invalidPhoto = new byte[] { 0x00 }; // Mock invalid photo

        // Act
        bool result = userMod.UpdateUserProfile("testUserId", validBio, invalidPhoto);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateBio_LessThan200Words_ReturnsTrue()
    {
        // Arrange
        var userMod = new UserModification();
        string validBio = "This is a valid bio.";

        // Act
        bool result = userMod.ValidateBio(validBio);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidateBio_MoreThan200Words_ReturnsFalse()
    {
        // Arrange
        var userMod = new UserModification();
        string invalidBio = new string('x', 1001); // Mock invalid bio

        // Act
        bool result = userMod.ValidateBio(invalidBio);

        // Assert
        Assert.IsFalse(result);
    }

    // Add these methods to the UserModificationTests class
    [TestMethod]
    public void ValidateProfilePhoto_ValidJPEG_ReturnsTrue()
    {
        // Arrange
        var userMod = new UserModification();
        byte[] validJPEGPhoto = { 0xFF, 0xD8 }; // Mock JPEG header

        // Act
        bool result = userMod.ValidateProfilePhoto(validJPEGPhoto);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidateProfilePhoto_ExceedsSizeLimit_ReturnsFalse()
    {
        // Arrange
        var userMod = new UserModification();
        byte[] oversizedPhoto = new byte[5 * 1024 * 1024 + 1]; // Mock photo exceeding size limit

        // Act
        bool result = userMod.ValidateProfilePhoto(oversizedPhoto);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateProfilePhoto_InvalidFormat_ReturnsFalse()
    {
        // Arrange
        var userMod = new UserModification();
        byte[] invalidFormatPhoto = { 0x00 }; // Mock invalid format

        // Act
        bool result = userMod.ValidateProfilePhoto(invalidFormatPhoto);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SaveProfileChangesToDatabase_SuccessfulUpdate_ReturnsTrue()
    {
        // Arrange
        var userMod = new UserModification();
        string userId = "testUserId";
        string updatedProfileData = "{ \"Bio\": \"Updated bio\" }";

        // Act
        bool result = userMod.SaveProfileChangesToDatabase(userId, updatedProfileData);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void SaveProfileChangesToDatabase_UnsuccessfulUpdate_ReturnsFalse()
    {
        // Arrange
        var userMod = new UserModification();
        string userId = "testUserId";
        string updatedProfileData = "{ \"Bio\": \"Updated bio\" }";

        // Act
        bool result = userMod.SaveProfileChangesToDatabase(userId, updatedProfileData);

        // Assert
        Assert.IsFalse(result);
    }
}
