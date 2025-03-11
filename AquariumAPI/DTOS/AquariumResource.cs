using System.ComponentModel.DataAnnotations;
using DAL;

namespace AquariumAPI.DTOS;

public class AquariumResource
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Depth must be greater than 0")]
    public double Depth { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Height must be greater than 0")]
    public double Height { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Length must be greater than 0")]
    public double Length { get; set; }

    [Required]
    public WaterType WaterType { get; set; }
}