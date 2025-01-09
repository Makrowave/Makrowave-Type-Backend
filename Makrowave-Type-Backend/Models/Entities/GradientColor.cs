using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Models.Entities;

public class GradientColor
{
    public int Id { get; set; }
    public required Guid UserThemeId { get; set; }
    [Required]
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format")]
    public required string Color { get; set; } = null!;

    public UserTheme? Theme { get; set; }
}