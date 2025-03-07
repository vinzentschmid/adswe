using DAL;
using Services.Authentication;

namespace Services.Models;

public class UserResponse
{
    public User User { get; set; }
    
    public AuthenticationInformation AuthenticationInformation { get; set; }
}