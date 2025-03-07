using System.Security.Claims;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AquariumAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BaseController<T> : ControllerBase where T : Entity
{
    protected IService<T> Service = null;

    public BaseController(IService<T> service, IHttpContextAccessor httpContextAccessor)
    {
        this.Service = service;
        Task model = this.Service.SetModelState(this.ModelState);
        model.Wait();

        if (httpContextAccessor.HttpContext != null)
        {
            if (httpContextAccessor.HttpContext.User != null)
            {
                var ClaimsPrincipal = httpContextAccessor.HttpContext.User;
                IEnumerable<Claim> claims = ClaimsPrincipal.Claims;
                
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (email == null) return;
                Task task2 = this.Service.Load(email);
                task2.Wait();
            }
        }
    }
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<T> Get(String id)
    {
        return await this.Service.Get(id);
    }
}