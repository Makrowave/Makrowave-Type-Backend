namespace Makrowave_Type_Backend.Dtos;

public class DailyScoreDto
{
    public required string Username { get; set; } = null!;
    public int Time {get; set;}
    public float Accuracy {get; set;}
    public int Score {get; set;}
}