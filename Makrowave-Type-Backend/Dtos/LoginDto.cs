using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Dtos;

public class LoginDto
{

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
}