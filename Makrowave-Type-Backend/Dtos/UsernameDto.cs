using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Dtos;

public class UsernameDto
{
    [Required] 
    [RegularExpression(Regexes.Name, ErrorMessage = "Invalid username")]
    public string Username { get; set; } = null!;
}