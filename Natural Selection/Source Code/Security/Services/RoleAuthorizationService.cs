using System.Generic.Collections;
using System.

public class RoleAuthorizationService : IRoleAuthorization
{
    public bool HasError;
    public bool CanRetry;
    public ClaimsPrincipal _Principal;
    public string fetchedHashPrincipal;

    public RoleAuthorizationService(ClaimsPrincipal Principal)
    {
        _Principal = Principal;
    }
    public bool ValidatePrincipal(ClaimsPrincipal Principal, string fetchedHashPrincipal)
    {

    }
    public bool IsAuthorized(HashSet<string> authRoles)
    {

    }
}