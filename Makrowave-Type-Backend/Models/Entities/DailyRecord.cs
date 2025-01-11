using System.ComponentModel.DataAnnotations;

namespace Makrowave_Type_Backend.Models.Entities;

public class DailyRecord
{
    public long Id { get; set; }
    public required Guid UserId { get; set; }
    public required DateTime Date { get; set; }
    public int Time { get; set; }
    public float Score { get; set; }
    [Range(0f,1f,ErrorMessage="Accuracy must be between 0 and 1.")]
    public float Accuracy { get; set; }

    public User? User { get; set; }
}