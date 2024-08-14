using System.ComponentModel.DataAnnotations;

namespace MealAPI.Models;

public class MealForCreationDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)] 
    public required string Name { get;set; }
}
