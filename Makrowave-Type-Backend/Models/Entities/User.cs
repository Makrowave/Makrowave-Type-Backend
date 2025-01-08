namespace Makrowave_Type_Backend.Models.Entities;

public class User
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }

    public UserTheme? Theme { get; set; }
    public ICollection<Session> Sessions { get; set; } = [];
    public ICollection<DailyRecord> DailyRecords { get; set; } = [];
}