using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class ValidationService : IHashService, IOTPService
{
    private readonly MyDbContext _dbContext;
    private readonly ILogger<ValidationService> _logger;
    public string UserIdentity { get; private set; }
    public string? Proof { get; private set; }
    private readonly string UserSalt;

    //Constructor to initialize the ValidationService with the user identity and an optional proof
    public ValidationService(MyDbContext dbContext, ILogger<ValidationService> logger, string userIdentity, string? proof = null)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        UserIdentity = userIdentity;
        Proof = proof;
        //Retrieve the user-specific salt during initialization for HashValue() to use
        UserSalt = GetSaltAsync(userIdentity); 

        //Log
    }

    //Creates OTP, hashes OTP, stores hashed OTP, returns plain OTP to send to user's email
    public string OTPCreation(string userIdentity)
    {
        //Create OTP
        var otp = CreateOTP();

        //Hash the OTP with the user-specific salt
        var otpHash = HashValue(otp);

        //Save the hash in the DB
        InsertHashOTPAsync(otpHash, userIdentity);

        //Log

        //Return the plain OTP to be sent to the user via email
        return otp;
    }


    //OTP is 8 Chars consisting of:
    //A-Z
    //a-z
    //0-9
    private string CreateOTP()
    {
        const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new RNGCryptoServiceProvider();
        var otpChars = new char[8]; //OTP length of 8 characters.

        //Generate a random index for each character in the OTP.
        byte[] randomBytes = new byte[1];
        for (int i = 0; i < otpChars.Length; i++)
        {
            random.GetBytes(randomBytes);
            var index = randomBytes[0] % validCharacters.Length;
            otpChars[i] = validCharacters[index];
        }

        otp = new string(otpChars);

        //log

        return otp;
    }

    private async Task<string> GetSaltAsync(string userIdentity)
    {
        var salt = await _dbContext.UserAccounts
            .AsNoTracking() //Use AsNoTracking if you do not intend to update the entity in the same context.
            .Where(u => u.identity == userIdentity)
            .Select(u => u.Salt)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(salt))
        {
            throw new InvalidOperationException("User account not found or salt is not set.");
        }

        //log
        return salt;
    }


    public string HashValue(string value)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(UserSalt)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            //log
            return Convert.ToBase64String(hash);
        }
    }

    //Async insert hash OTP into userAccount table
    private async Task<bool> InsertHashOTPAsync(string otpHash, string userIdentity)
    {
        //Insert the hash into the database.
        var userAccount = await _dbContext.UserAccounts.SingleOrDefaultAsync(u => u.identity == userIdentity);
        if (userAccount != null)
        {
            userAccount.hashedOTP = otpHash;
            await _dbContext.SaveChangesAsync();
            //Log
            return true;
        }
        //Log
        return false;
    }

    public bool OTPValidation(string userIdentity, string proof)
    {
        //Fetch the hashed OTP from the datastore
        string storedHashedOtp = HashedOTPFetchingAsync(userIdentity);

        //Hash the proof provided by the user
        string hashedProof = HashValue(proof);

        //Compare the stored hashed OTP with the hashed proof
        bool validity = storedHashedOtp == hashedProof;

        //Log

        //Return validity
        return validity;
    }


    //Asynchronously retrieves the hashed OTP for the specified user identity from the database.
    private async Task<string> HashedOTPFetchingAsync(string userIdentity)
    {
        var hashOTP = await _dbContext.UserAccounts
            .AsNoTracking() //Use AsNoTracking if you do not intend to update the entity in the same context.
            .Where(u => u.identity == userIdentity)
            .Select(u => u.hashedOTP)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(hashOTP))
        {
            //log
            throw new InvalidOperationException("User account not found or OTP hash is not set.");
        }

        //log
        return hashOTP;
    }
}