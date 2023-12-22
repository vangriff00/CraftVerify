using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountRecoveryService
{
    internal interface IRecoverAccount
    {
        public bool RecoverUserAccount(string userHash, string newStatus);

        public bool LogRecoveryRequest(string userHash);
    }
}
