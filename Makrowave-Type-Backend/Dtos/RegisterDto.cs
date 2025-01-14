using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Dtos;
//Incoming
public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    [RegularExpression(Regexes.Name, ErrorMessage = "Invalid username")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(Regexes.Password, ErrorMessage = "Invalid password")]
    public string Password { get; set; } = null!;
}