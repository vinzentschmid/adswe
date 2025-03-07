using DAL;

namespace Services.Authentication;

public interface IAuthentication
{
    public String GetEmailByToken(String token);
    
    public String GetClaim(String token, String claimType);
    
    public Task<AuthenticationInformation> Authenticate(User user);
    
    public Task<bool> ValidateCurrentToken(String token);
    
}