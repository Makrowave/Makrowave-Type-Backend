using System.Text.RegularExpressions;
using Makrowave_Type_Backend.Dtos;

namespace Makrowave_Type_Backend.Validators;

public class ThemeValidator
{
    public static bool IsValidTheme(UserThemeDto theme)
    {
        return IsValidHex(theme.UiText)
               && IsValidHex(theme.UiBackground)
               && IsValidHex(theme.TextIncomplete)
               && IsValidHex(theme.TextComplete)
               && IsValidHex(theme.TextIncorrect)
               && IsValidHex(theme.InactiveKey)
               && IsValidHex(theme.InactiveText)
               && IsValidHex(theme.ActiveText)
               && !theme.Gradient.Exists(color => !IsValidHex(color));
    }
    
    public static bool IsValidHex(string hex)
    {
        return Regex.IsMatch(hex, "^#[0-9A-F]{6}$");
    }
}