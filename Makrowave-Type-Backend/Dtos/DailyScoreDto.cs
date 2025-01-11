namespace Makrowave_Type_Backend.Dtos;

//Outgoing
public class DailyScoreDto
{
    public required string Username { get; set; } = null!;
    public int Time {get; set;}
    public float Accuracy {get; set;}
    public float Score {get; set;}
    public int Place { get; set; }
}