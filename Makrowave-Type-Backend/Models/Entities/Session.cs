namespace Makrowave_Type_Backend.Models.Entities;

public class Session
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public required DateTime ExpirationDate { get; set; }
    
    public User? User { get; set; }
}