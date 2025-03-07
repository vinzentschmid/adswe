using System.ComponentModel.DataAnnotations;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace AquariumAPI.Controllers;

public class UserController : BaseController<User>
{
    private UserService userService;
    public UserController(IGlobalService service, IHttpContextAccessor httpContextAccessor) : base(service.UserService, httpContextAccessor)
    {
        this.userService = service.UserService;
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<ItemResponseModel<UserResponse>>> Login([FromBody] [Required]LoginRequest request)
    {
        ItemResponseModel<UserResponse> response = await ((UserService)Service).Login(request);
        if (response.HasError == false)
        {
            return Ok(response);

        }

        return new UnauthorizedResult();
    }
    
}