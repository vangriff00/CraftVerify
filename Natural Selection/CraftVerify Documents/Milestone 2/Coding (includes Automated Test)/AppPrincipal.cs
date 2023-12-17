using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
 
public class AppPrincipal : ClaimsPrincipal
{
    public IDictionary<string, string> Claims { get; set; }
 
    public IIdentity? Identity => throw new NotImplementedException();
 
    public bool IsInRole(string role)
    {
        throw new NotImplementedException();
    }
}
