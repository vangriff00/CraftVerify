using ValidationService;
using System.Security.Claims;
using System;
using System.Data.SqlClient;

public class AuthenticationService : IRoleAuthenticator
{
    /// <summary>
    /// Authenticates the given authentication request.
    /// </summary>
    /// <param name="authRequest">The authentication request.</param>
    /// <returns>The authenticated principal.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the authRequest is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the authRequest is invalid.</exception>
    public bool hasError;
    public bool canRetry;
    public ClaimsPrincipal principal;
    public string claimsPrincipalHash;
    public ClaimsPrincipal Authenticate(AuthenticationRequest authRequest)
    {
        // Early exit
        #region Validate arguments
        ClaimsPrincipal appPrincipal = null;
        //Test if OTP is valid
        hashProof = HashValue(authResponse.proof);
        if (ValidationService.OTPValidation(authRequest.UserIdentity, hashProof) is false)
        {
            
        }
        #endregion

        try
        {
            
            // Step 1: Validate auth request
                //verify user isn't already authenticated
            
            // Step 2: Populate app principal object
            var claims = new Dictionary<string, string>();
            // ... (possibly other code to populate claims)
 
            appPrincipal = new AppPrincipal()
            {
                UserIdentity = authRequest.UserIdentity,
                Claims = claims
            }
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetBaseException().Message;
            // logging errorMessage;
        }
 
        return AuthenticationRequest;
    }
    private bool InsertPrincipal(string claims, string userIdentity, string claimsPrincipalHash)
    {
        int rows = 0;
        string connection_string = 'Server=localhost;Database=CraftVerify;Trusted_Connection=True;";
        using (SqlConnection connection = new SqlConnection(connection_string))
        {
            connection.Open();
            string insertQuery = "INSERT INTO claims_principal (claim, identity, hash_principal) VALUES (@claim, @identity, @hash_principal)";
            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                
                insertCommand.Parameters.AddWithValue("@claim", claims);
                insertCommand.Parameters.AddWithValue("@identity", userIdentity);
                insertCommand.Parameters.AddWithValue("@hash_principal", claimsPrincipalHash);

                try
                {
                    rows = insertCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
        }
        if (rows == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsAlreadyAuthenticated(string userIdentity)
    {
        string connection_string = 'Server=localhost;Database=CraftVerify;Trusted_Connection=True;";
        using (SqlConnection connection = new SqlConnection(connection_string))
        {
            connection.Open();

            string queru = "SELECT identity FROM claims_principal WHERE userIdentity == identity";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userIdentity", userIdentity);
                object result = command.ExecuteScalar();

                if (result != null && Convert.ToInt32(result) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
    }
}