public class CraftVerifyAuthService : IAuthenticator, IAuthorizer
{
    /// <summary>
    /// Authenticates the given authentication request.
    /// </summary>
    /// <param name="authRequest">The authentication request.</param>
    /// <returns>The authenticated principal.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the authRequest is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the authRequest is invalid.</exception>
    public IPrincipal Authenticate(AuthenticationRequest authRequest)
    {
        // Early exit
        #region Validate arguments
        AppPrincipal appPrincipal = null;
        #endregion
 
        try
        {
            // Step 1: Validate auth request
 
            // Step 2: Populate app principal object
            var claims = new Dictionary<string, string>();
            // ... (possibly other code to populate claims)
 
            appPrincipal = new AppPrincipal()
            {
                UserIdentity = authRequest.UserIdentity,
                Claims = claims
            };
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetBaseException().Message;
            // logging errorMessage;
        }
 
        return appPrincipal;
    }
 
    public bool IsAuthorize(AppPrincipal currentPrincipal, IDictionary<string, string> requiredClaims)
    {
        // Dictionary<string, string> example = new { "RoleName", "Admin" }
        // Key - RoleName, Value - Admin
 
        foreach (var claim in requiredClaims)
        {
            if (!currentPrincipal.Claims.Contains(claim))
            {
                return false;
            }
        }
 
        return true;
    }
}