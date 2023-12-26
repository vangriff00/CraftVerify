using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security_Library
{
    internal interface IAuthorizer
    {
        bool IsAuthorized(AppPrincipal currentPrincipal, IDictionary<string, string> claims);
    }

}
