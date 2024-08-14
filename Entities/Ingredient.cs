using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MealAPI.Entities;

public class Ingredient
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public Ingredient()
    {  }

    [SetsRequiredMembers]
    public Ingredient(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
