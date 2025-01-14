using System.ComponentModel.DataAnnotations;

namespace Makrowave_Type_Backend.Dtos;

public class UsernameDto
{
    [Required] public string Username { get; set; } = null!;
}