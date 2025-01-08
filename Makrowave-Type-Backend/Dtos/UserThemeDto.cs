namespace Makrowave_Type_Backend.Dtos;

public class UserThemeDto
{
    public required string UiText { get; set; }
    public required string UiBackground { get; set; }

    public required string TextIncomplete { get; set; }
    public required string TextComplete { get; set; }
    public required string TextIncorrect { get; set; }

    public required string InactiveKey { get; set; }
    public required string InactiveText { get; set; }
    public required string ActiveText { get; set; }
    public required List<string> Gradient { get; set; }
}