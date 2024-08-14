namespace MealAPI.Models;

public class IngredientDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid MealId { get; set; }    
}
