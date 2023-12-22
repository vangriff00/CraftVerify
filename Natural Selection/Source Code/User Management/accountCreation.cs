using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;



namespace TeamNatural.CraftVerify.UserManager;


public interface IAccountCreation
    {
        int UserID { get; }
        string Email { get; set; }
        DateTime DOB { get; }
        string UserRole { get; set; }
        string UserStatus { get; set; }

        void SetAdditionalAttribute(string key, object value);
        object GetAdditionalAttribute(string key);
        
    }

    public class AccountCreation : IAccountCreation
    {
        // Fixed properties
        public int UserID { get; set; }
        public string UserHash { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string UserRole { get; set; }
        public string UserStatus { get; set; }

        // Dictionary for additional, dynamic attributes
        public Dictionary<string, object> AdditionalAttributes { get; set; }


        private readonly IDBInserter _dbInserter;

        public AccountCreation(IDBInserter dbInserter)
        {
        _dbInserter = dbInserter;
        AdditionalAttributes = new Dictionary<string, object>();
        }



        public AccountCreation()
        {
            AdditionalAttributes = new Dictionary<string, object>();
        }

        // Method to safely add or update additional attributes
       
        public void SetAdditionalAttribute(string key, object value)
        {
            throw new NotImplementedException();
        }

        public object GetAdditionalAttribute(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertUserAsync()
        {
        return await _dbInserter.InsertUserIntoTwoTablesAsync("UserAccount", "UserProfile", this);
        }

}












