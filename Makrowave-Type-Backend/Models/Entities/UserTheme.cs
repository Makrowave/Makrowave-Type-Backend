namespace Makrowave_Type_Backend.Models.Entities;

public class UserTheme
{
    public Guid UserThemeId { get; set; }
    public required string UiText { get; set; }
    public required string UiBackground { get; set; }

    public required string TextIncomplete { get; set; }
    public required string TextComplete { get; set; }
    public required string TextIncorrect { get; set; }

    public required string InactiveKey { get; set; }
    public required string InactiveText { get; set; }
    public required string ActiveText { get; set; }

    public User? User { get; set; }
    public ICollection<GradientColor> GradientColors { get; set; } = [];
}