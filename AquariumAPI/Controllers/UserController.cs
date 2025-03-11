using System.ComponentModel.DataAnnotations;
using AquariumAPI.DTOS;
using DAL;
using DAL.Utils;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace AquariumAPI.Controllers;

public class UserController(IGlobalService service, IHttpContextAccessor httpContextAccessor)
    : BaseController<User>(service.UserService, httpContextAccessor)
{
    private UserService userService = service.UserService;

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
    
    [HttpPost("Register")]
    public async Task<ActionResult<ItemResponseModel<User>>> Register([FromBody] [Required] UserResource user)
    {
        if (user == null)
        {
            return BadRequest("User cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var passwordHasher = new PasswordHasher();
        var newUser = new User
        {
            Email = user.Email,
            HashedPassword = passwordHasher.Hash(user.Password)
        };
        var response = await userService.Create(newUser);

        if (response.HasError)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("Update/{id}")]
    public async Task<ActionResult<ItemResponseModel<User>>> Update(string id, [FromBody] [Required] UserResource user)
    {
        if (user == null)
        {
            return BadRequest("User cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var passwordHasher = new PasswordHasher();

        var updatedUser = new User
        {
            Email = user.Email,
            HashedPassword = passwordHasher.Hash(user.Password)

        };
        var response = await userService.Update(id, updatedUser);

        if (response.HasError)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}