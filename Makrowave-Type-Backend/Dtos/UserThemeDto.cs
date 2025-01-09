using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Dtos;
//Incoming and outgoing
public class UserThemeDto
{
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string UiText { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string UiBackground { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string TextIncomplete { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string TextComplete { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string TextIncorrect { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string InactiveKey { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string InactiveText { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [RegularExpression(Regexes.Hex, ErrorMessage = "{0} is not a valid hex number.")]
    public required string ActiveText { get; set; } = null!;
    [Required(ErrorMessage = "{0} is missing or empty.")]
    [ColorList]
    public required List<string> Gradient { get; set; } = null!;
}