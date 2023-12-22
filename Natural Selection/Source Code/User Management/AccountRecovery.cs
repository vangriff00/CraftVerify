using AccountRecoveryService;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CraftVerify.NaturalSelection.UserManagment
{
    public class AccountRecoveryService : IRecoverAccount
    {
        //Need a DAO

        private string userHash;
        private string newStatus;

        public AccountRecoveryService(string userHash, string newStatus)
        {
            this.userHash = userHash;
            this.newStatus = newStatus;
        }

        public bool RecoverUserAccount(string userHash, string newStatus)
        {
            if (IsUserHashValid(userHash) && IsStatusValid(newStatus))
            {
                // use DAO to update the user with the userHash with the status change to newStatus
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool LogRecoveryRequest(string userHash)
        {
            Console.WriteLine("Work In Progress");
            return true;
        }

        public bool IsStatusValid(string userStatus)
        {
            if ((!(string.IsNullOrEmpty(userStatus))) && ((userStatus == "Disable") || (userStatus == "Enable")))
            {
                return true;
            }
            return false;
        }

        public bool IsUserHashValid(string userHash)
        {
            if ((!(string.IsNullOrEmpty(userHash))) && (Regex.IsMatch(userHash, "^[a-fA-F0-9]{64}$")))
            {
               return true;
                    
            }
           return false;
        }
    }
}