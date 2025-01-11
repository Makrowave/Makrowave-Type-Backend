namespace Makrowave_Type_Backend.Dtos;
//Outgoing
public class AllTimeScoreDto
{
    public required string Username { get; set; } = null!;
    public int Wins { get; set; }
    public int Place { get; set; }
}