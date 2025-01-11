using System.ComponentModel.DataAnnotations;

namespace Makrowave_Type_Backend.Dtos;

public class ChallengeDto
{
    [Required]
    public int Time { get; set; }
    [Required]
    public float Score { get; set; }
    [Required]
    [Range(0f,1f,ErrorMessage="Accuracy must be between 0 and 1.")]
    public float Accuracy { get; set; }
}