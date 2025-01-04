using Makrowave_Type_Backend.Models.Entities;

namespace Makrowave_Type_Backend.Models;

public class DefaultTheme
{
    private readonly string _uiText;
    private readonly string _uiBackground;

    private readonly string _textIncomplete;
    private readonly string _textComplete;
    private readonly string _textIncorrect;

    private readonly string _inactiveKey;
    private readonly string _inactiveText;
    private readonly string _activeText;

    private readonly List<string> _gradient;
    public DefaultTheme(IConfiguration config)
    {
        _uiText = GetConfigValue(config, "UIText");
        _uiBackground = GetConfigValue(config, "UIBackground");
        _textIncomplete = GetConfigValue(config, "TextIncomplete");
        _textComplete = GetConfigValue(config, "TextComplete");
        _textIncorrect = GetConfigValue(config, "TextIncorrect");
        _inactiveKey = GetConfigValue(config, "InactiveKey");
        _inactiveText = GetConfigValue(config, "InactiveText");
        _activeText = GetConfigValue(config, "ActiveText");
        var gradient = config.GetSection("Theme.Gradient").Get<List<string>>() ?? throw new InvalidOperationException();
        if(gradient == null || gradient.Count > 2) throw new InvalidOperationException();
        _gradient = gradient;
    }

    public UserTheme GenerateDefaultUserTheme(Guid userId)
    {
        return new UserTheme()
        {
            UiText = _uiText,
            UiBackground = _uiBackground,
            TextIncomplete = _textIncomplete,
            TextComplete = _textComplete,
            TextIncorrect = _textIncorrect,
            InactiveKey = _inactiveKey,
            InactiveText = _inactiveText,
            ActiveText = _activeText,
            GradientColors = _gradient.Select((color, index) =>
                new GradientColor() { Color = color, Id = index, UserThemeId = userId }).ToList()
        };
    }

    private static string GetConfigValue(IConfiguration configuration, string key)
    {
        return configuration["Theme.UiText"] ?? throw new InvalidOperationException();
    }
}