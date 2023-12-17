using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security_Library
{
    public interface IAuthentication
    {
        //Or can be string userID, string proof (user we want to authenticate and the proof of user
        //ValueTuple return
        //AuthenticationResponse Authenticate(AuthenticationRequest authRequest);
        AppPrincipal Authenticate(AuthenticationRequest);
    }
}

