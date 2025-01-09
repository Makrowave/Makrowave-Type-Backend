using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Dtos;
//Incoming
public class AuthDto
{
    [Required(ErrorMessage = "Username is required")]
    [RegularExpression(Regexes.Name, ErrorMessage = "Invalid username format")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(Regexes.Password, ErrorMessage = "Invalid password format")]
    public string Password { get; set; } = null!;
}