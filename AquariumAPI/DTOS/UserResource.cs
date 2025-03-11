using System.ComponentModel.DataAnnotations;

namespace AquariumAPI.DTOS;

public class UserResource
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}