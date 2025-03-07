using Microsoft.AspNetCore.Http;

namespace Services;

public class LoginRequest
{
    public String Username { get; set; }
    public String Password { get; set; }
}

public class PictureRequest
{
    public String Description { get; set; }
    public IFormFile FormFile { get; set; }

}