using System.Security.Claims;
using System.Security.Principal;

namesapce Security_Library;

public class MYClaimsPrincipal : ClaimsPrincipal 
{
    public IEnumerable Claims{get; set;}
    public ClaimsPrincipal current {get; set;}
    
    public MYClaimsPrincipal()
    {
        Claims = ClaimsPrincipal.Claims;
        current = MYClaimsPrincipal;
    }
}