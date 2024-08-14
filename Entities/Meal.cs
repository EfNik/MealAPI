using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MealAPI.Entities;

public class Meal 
{ 
    [Key]
    public Guid Id { get; set; }

    // "required" modifier: compiler guarantees Name is initialized when the Meal is instantiated.
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public Meal()
    { 
    }

    [SetsRequiredMembers]
    public Meal(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
