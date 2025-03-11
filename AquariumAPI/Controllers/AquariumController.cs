using System.Security.Claims;
using AquariumAPI.DTOS;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace AquariumAPI.Controllers;

public class AquariumController(IGlobalService globalService, IHttpContextAccessor httpContextAccessor)
    : BaseController<Aquarium>(globalService.AquariumService, httpContextAccessor)
{
    private readonly AquariumService _aquariumService = globalService.AquariumService;
    
    [Authorize]
    [HttpPost("CreateAquarium")]
    public async Task<ActionResult<ItemResponseModel<Aquarium>>> CreateAquarium([FromBody] AquariumResource resource)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var aquarium = new Aquarium
        {
            Name = resource.Name,
            Depth = resource.Depth,
            Height = resource.Height,
            Length = resource.Length,
            WaterType = resource.WaterType
        };

        
        await _aquariumService.SetModelState(this.ModelState);
        
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("User not found.");
        }

        await _aquariumService.Load(email);
        
        
        var response = await _aquariumService.Create(aquarium);

        if (response.HasError)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [Authorize]
    [HttpPut("UpdateAquarium/{id}")]
    public async Task<ActionResult<ItemResponseModel<AquariumResource>>> UpdateAquarium(string id, [FromBody] AquariumResource resource)
    {
        var aquarium = new Aquarium
        {
            Name = resource.Name,
            Depth = resource.Depth,
            Height = resource.Height,
            Length = resource.Length,
            WaterType = resource.WaterType
        };
        
        var responseModel = await _aquariumService.Update(id, aquarium);
        if (responseModel.HasError)
        {
            return BadRequest(responseModel);
        }
        
        return Ok(responseModel);
    }
    
    [Authorize]
    [HttpPost("Coral")]
    public async Task<ActionResult<ItemResponseModel<CoralResource>>> CreateCoral([FromBody] CoralResource resource)
    {
        if (resource == null)
        {
            return BadRequest("CoralResource cannot be null.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var coral = new AquariumItem()
        {
            Name = resource.Name,
            Species = resource.Species,
        };

        var response = await globalService.CoralService.Create(coral);

        if (response.HasError)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [Authorize]
    [HttpPut("UpdateCoral/{id}")]
    public async Task<ActionResult<ItemResponseModel<CoralResource>>> UpdateCoral(string id, [FromBody] CoralResource resource)
    {
        var coral = new AquariumItem()
        {
            Name = resource.Name,
            Species = resource.Species,
        };

        var responseModel = await globalService.CoralService.Update(id, coral);
        
        if (responseModel.HasError)
        {
            return BadRequest(responseModel);
        }
        
        return Ok(responseModel);
    }
    
    [Authorize]
    [HttpPost("Animal")]
    public async Task<ActionResult<ItemResponseModel<AnimalResource>>> CreateAnimal([FromBody] AnimalResource resource)
    {
        var animal = new AquariumItem
        {
            Name = resource.Name,
            Species = resource.Species,
        };

        var responseModel = await globalService.AnimalService.Create(animal);
        
        if (responseModel.HasError)
        {
            return BadRequest(responseModel);
        }
        return Ok(responseModel);
    }
    
    [Authorize]
    [HttpPut("UpdateAnimal/{id}")]
    public async Task<ActionResult<ItemResponseModel<AnimalResource>>> UpdateAnimal(string id, [FromBody] AnimalResource resource)
    {
        var animal = new AquariumItem
        {
            Name = resource.Name,
            Species = resource.Species,
        }; 
        
        var responseModel = await globalService.CoralService.Update(id, animal);
        
        if (responseModel.HasError)
        {
            return BadRequest(responseModel);
        }

        return Ok(responseModel);
    }
    
    [Authorize]
    [HttpGet("ForUser")]
    public async Task<ActionResult<ItemResponseModel<AnimalResource>>> GetAquariumsForUser()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("User email not found.");
        }

        var response = await globalService.AquariumService.GetAllAquariumsForUser(email);        
        if (response == null)
        {
            return BadRequest("No aquariums found for user.");
        }
        return Ok(response);
    }


}

