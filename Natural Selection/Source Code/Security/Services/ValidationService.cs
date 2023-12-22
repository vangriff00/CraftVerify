using System.Security.Cryptography;

public class ValidationService : IHashService, IOTPService
{
    public string UserIdentity;
    public string Proof;
    private readonly string UserSalt;

    public string OTPCreation(string UserIdentity)
    {

    }
    private string CreateOTP()
    {

    }
    private string GetSalt(string UserIdentity)
    {

    }
    public string HashValue(string value)
    {

    }
    private string InsertHashOTP(string UserHash, string hashProof)
    {

    }
    public bool OTPValidation(string UserIdentity, string hashProof)
    {

    }
    private string OTPFetching(string UserIdentity)
    {
        
    }
}