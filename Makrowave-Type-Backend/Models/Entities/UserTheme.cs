using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Models.Entities;

public class UserTheme
{
    public Guid UserThemeId { get; set; }
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string UiText { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string UiBackground { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string TextIncomplete { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string TextComplete { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string TextIncorrect { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]

    public required string InactiveKey { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string InactiveText { get; set; } = null!;
    [Required] 
    [RegularExpression(Regexes.Hex, ErrorMessage = "Invalid color format for {0}")]
    public required string ActiveText { get; set; } = null!;

    public User? User { get; set; }
    public ICollection<GradientColor> GradientColors { get; set; } = [];
}