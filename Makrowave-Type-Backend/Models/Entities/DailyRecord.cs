namespace Makrowave_Type_Backend.Models.Entities;

public class DailyRecord
{
    public long Id { get; set; }
    public required Guid UserId { get; set; }
    public required DateTime Date { get; set; }
    public int Time { get; set; }
    public int Score { get; set; }
    public float Accuracy { get; set; }

    public User? User { get; set; }
}