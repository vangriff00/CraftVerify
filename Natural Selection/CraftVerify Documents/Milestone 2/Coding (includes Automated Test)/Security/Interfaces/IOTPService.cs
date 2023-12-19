public interface IOTPService
{
    string UserIdentity {get; set;}
    string OTPCreation(string UserIdentity);
    bool OTPValidation(string UserIdentity, string Proof);
}