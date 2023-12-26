using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Threading.Tasks;




public class Services
{


    public bool IsValidEmail(string email)
    {
        /*
         •	Valid email consists of:
            i.	Minimum of 3 characters
            ii.	Must be in the format: <valid_characters>@<valid_characters>
            iii.	a-z (case insensitive)
            iv.	0-9
            v.	May have special characters 
                1.	.
                2.	-
         */

        // allowedEmailLength = 64
        // emailMinLength = 8 if we use email as username
        // allowedEmailPattern  should be = @"^[a-zA-Z0-9@.-]*$"



        return (!IsNullString(email) && IsValidLength(email, 8, 64) && IsValidDigit(email, @"^[a-zA-Z0-9@.-]*$") && IsValidPosition(email));
    }

    public bool IsValidDate(string dobString, int maxLengthDOB, DateTime minDate, DateTime maxDate)
    {
        /*
         Valid date of births begins January 1st, 1970 and ends current date.
         minDate = DateTime(1970, 1, 1);
         maxDate = DateTime.Now;
         maxLengthDOB = 10;
         */
        if (dobString.Length > maxLengthDOB)                        // For security to prevent overflow input
        {
            return false;
        }
        else if (DateTime.TryParse(dobString, out DateTime dob))   // Try parsing the input string to a DateTime object
        {
            return (dob >= minDate && dob <= maxDate);           // Check if the parsed date falls within the valid range
        }

        return false;                                           // If parsing fails, the input is not a valid date
    }

    public bool IsNullString(string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public bool IsValidLength(string email, int minLength, int maxLength)
    {
        if (IsNullString(email))
        {
            return false; // null string is considered invalid
        }

        int length = email.Length;
        return length >= minLength && length <= maxLength;
    }

    public bool IsValidDigit(string email, string allowedEmailPattern)
    {
        if (IsNullString(email))
        {
            return false; // Invalid if the email is null or empty
        }

        return Regex.IsMatch(email, allowedEmailPattern);
    }

    public bool IsValidPosition(string email)
    {
        if (IsNullString(email))
        {
            return false; // Invalid if the email is null or empty
        }

        int atIndex = email.IndexOf('@');

        // Check if '@' is not at the start, not at the end, and occurs only once
        return atIndex > 0 && atIndex < email.Length - 1 && email.LastIndexOf('@') == atIndex;
    }



    public int GenerateUserID()
    {
        byte[] randomNumber = new byte[4]; // int is 4 bytes
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomNumber);
        }

        // Convert byte array to an int
        int value = BitConverter.ToInt32(randomNumber, 0);

        // Ensure a positive number that's within the range of 9 digits
        int idLength = 9;
        int minValue = (int)Math.Pow(10, idLength - 1);
        int maxValue = (int)Math.Pow(10, idLength) - 1;

        return Math.Abs(value % (maxValue - minValue)) + minValue;
    }

    public string ReadPepper()
    {
        return "Hello491A";
    }

    public string HashSHA256(string email, string nullSalt, string pepper)
    {
        // Combine the email and pepper. The nullSalt is not used.
        string combinedString = email + nullSalt + pepper;

        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Compute the hash of the combined string.
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedString));

            // Convert the byte array to a hexadecimal string.
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public string CreateEmailPepperHash(string email)
    {
        return HashSHA256(email, "", ReadPepper());
    }

    public bool IsValidUserRole(string roleString)
    {
        // role would be "admin" or "regular" length from 5 to 10 with @"^[a-zA-Z0-9]*$" as pattern
        return (!IsNullString(roleString) && IsValidLength(roleString, 5, 10) && IsValidDigit(roleString, @"^[a-zA-Z0-9]*$"));
    }
}
