using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{

    public class account
    {
        private int accountId;
        private string email;
        private DateTime dateOfBirth;
        private string username;
        private string role;
        private Dictionary<string, object> additionalAttributes; // Store additional attributes

        // Constructor to initialize the core account attributes and the additional attributes dictionary
        public account(int accountId, string email, DateTime dateOfBirth, string username, string role)
        {
            this.accountId = accountId;
            this.email = email;
            this.dateOfBirth = dateOfBirth;
            this.username = username;
            this.role = role;
            this.additionalAttributes = new Dictionary<string, object>();
        }

        // Method to add or update additional attributes (including userhash) in the dictionary
        public void setAdditionalAttribute(string attributeName, object attributeValue)
        {
            additionalAttributes[attributeName] = attributeValue;
        }

        // Method to get the value of an additional attribute from the dictionary
        public object getAdditionalAttribute(string attributeName)
        {
            if (additionalAttributes.ContainsKey(attributeName))
            {
                return additionalAttributes[attributeName];
            }
            else
            {
                return null; // Handle the case where the attribute does not exist
            }
        }

    }
}


