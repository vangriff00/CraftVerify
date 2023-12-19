using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security_Library
{
    
    internal interface IRoleAuthenticator : IAuthenticator
    {
        private readonly AuthenticationRequest;
        bool Authenticate(AppPrincipal currentPrincipal, IDictionary<string, string> claims);
    }

}