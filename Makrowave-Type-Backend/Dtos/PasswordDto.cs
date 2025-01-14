using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Dtos;

public class PasswordDto
{
    [Required]
    [RegularExpression(Regexes.Password, ErrorMessage = "Invalid password")]
    public string Password { get; set; } = null!;
}