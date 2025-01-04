namespace Makrowave_Type_Backend.Models.Entities;

public class GradientColor
{
    public int Id {get; set;}
    public required Guid UserThemeId {get; set;}
    public required string Color {get; set;}
    
    public required UserTheme Theme {get; set;}
}