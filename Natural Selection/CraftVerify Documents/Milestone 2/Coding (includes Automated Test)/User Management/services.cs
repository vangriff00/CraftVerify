using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ClassLibrary1
{
    public class services
    {
// adhere business rules
        public bool isValidEmail(string email)
        {
            if(nullStringCheck(email) || validLengthCheck(email, 3, 64) || validDigitCheck(username, @"^[a-zA-Z0-9@.-]*$")|| validPositionCheck(email))
            {
                //email in the correct format
                // checking email exist
            }
            else
                return false;        
        
        }

        public bool isValidDOB(string dobString)
        {
           /*
             Valid date of births begins January 1st, 1970 and ends current date.
             minDate = DateTime(1970, 1, 1);
             maxDate = DateTime.Now;
             maxLengthDOB = 10;
             */
            return validDate((dobString), 10, DateTime(1970,1,1), DateTime.now);
        }

        public bool isValidUsername(string username)
        {
            /*
             •	Valid usernames consists of:
                i.	Minimum of 8 characters.	
                ii.	a-z (case insensitive)
                iii.	0-9
                iv.	May have special characters 
                    1.	.
                    2.	-
                    3.  @
             */

            // allowedUsernameLength = 64
            // usernameMinLength = 8
            // allowedUsernamePattern  should be = @"^[a-zA-Z0-9@.-]*$"     a-Z, 0-9, @ . -


            return (nullStringCheck(username) || validLengthCheck(username, 8, 64) || validDigitCheck(username, @"^[a-zA-Z0-9@.-]*$"));
        }



// reusable services with no business rules

        public bool validDateCheck(string dobString, int maxLengthDOB, DateTime minDate, DateTime maxDate)
        {
            if (dobString.Length > maxLengthDOB || nullStringCheck(dobString))          // For security to prevent overflow input
            {
                return false;
            }
            else if (DateTime.TryParse(dobString, out DateTime dob))   // Try parsing the input string to a DateTime object
            {
                return (dob >= minDate && dob <= maxDate);           // Check if the parsed date falls within the valid range
            }

            return false;                                           // If parsing fails, the input is not a valid date
        }

        public bool nullStringCheck(string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public bool validLengthCheck(string email, int minLength, int maxLength)
        {
            if (nullStringCheck(email))
            {
                return false; // null string is considered invalid
            }

            int length = email.Length;
            return length >= minLength && length <= maxLength;
        }

        public bool validDigitCheck(string email, string allowedEmailPattern)
        {
            if (nullStringCheck(email))
            {
                return false; // Invalid if the email is null or empty
            }

            return Regex.IsMatch(email, allowedEmailPattern);
        }

        public bool validPositionCheck(string email)
        {
            if (nullStringCheck(email))
            {
                return false; // Invalid if the email is null or empty
            }

            int atIndex = email.IndexOf('@');

            // Check if '@' is not at the start, not at the end, and occurs only once
            return atIndex > 0 && atIndex < email.Length - 1 && email.LastIndexOf('@') == atIndex;
        }
     
        public static int userIDGenerator()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[4]; // 4 bytes for a 32-bit integer

                rng.GetBytes(buffer);
                int result = BitConverter.ToInt32(buffer, 0);

                // Ensure a non-negative number is returned
                return Math.Abs(result);
            }
        }



    }
}
