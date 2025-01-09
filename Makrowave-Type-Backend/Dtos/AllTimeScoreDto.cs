namespace Makrowave_Type_Backend.Dtos;

public class AllTimeScoreDto
{
    public required string Username { get; set; } = null!;
    public int Wins { get; set; }
}