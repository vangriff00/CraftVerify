public interface IRoleDeauthenticate : IDeauthenticate
{
    ClaimsPrincipal RoleAuthorizationService {get; set;}
    bool Deauthenticate(RoleAuthorizationService);

}