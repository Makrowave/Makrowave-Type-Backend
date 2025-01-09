using System.ComponentModel.DataAnnotations;
using Makrowave_Type_Backend.Validation;

namespace Makrowave_Type_Backend.Models.Entities;

public class User
{
    public Guid UserId { get; set; }
    
    [Required]
    [RegularExpression(Regexes.Name, ErrorMessage = "Invalid name format")]
    public required string Username { get; set; }
    [Required]
    public required string PasswordHash { get; set; }

    public UserTheme? Theme { get; set; }
    public ICollection<Session> Sessions { get; set; } = [];
    public ICollection<DailyRecord> DailyRecords { get; set; } = [];
}