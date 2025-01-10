using System.Security.Claims;
using Makrowave_Type_Backend.Dtos;
using Makrowave_Type_Backend.Models;
using Makrowave_Type_Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly DatabaseContext _dbContext;


    public SettingsController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;

    }

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpGet("theme")]
    public async Task<ActionResult<string>> GetUserTheme()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var theme = await _dbContext.UserThemes.FindAsync(userId);
        var gradientList = _dbContext.GradientColors.Where(color => color.UserThemeId == userId).OrderBy(color => color.Id).Select(color => color.Color).ToList();
        var result = new UserThemeDto()
        {
            UiText = theme.UiText,
            UiBackground = theme.UiBackground,
            TextIncomplete = theme.TextIncomplete,
            TextComplete = theme.TextComplete,
            TextIncorrect = theme.TextIncorrect,
            InactiveKey = theme.InactiveKey,
            InactiveText = theme.InactiveText,
            ActiveText = theme.ActiveText,
            Gradient = gradientList
        };
        return Ok(result);
    }
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPost("theme")]
    public async Task<ActionResult<string>> SaveUserTheme(UserThemeDto userTheme)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);

        if (!UserExists(userId) || !_dbContext.UserThemes.Any(theme => theme.UserThemeId == userId))
        {
            return NotFound("User does not exist");
        }

        var gradients = _dbContext.GradientColors.Where(color => color.UserThemeId == userId).ToList();
        _dbContext.GradientColors.RemoveRange(gradients);
        var theme = await _dbContext.UserThemes.FindAsync(userId);
        theme!.UiText = userTheme.UiText;
        theme.UiBackground = userTheme.UiBackground;
        theme.TextIncomplete = userTheme.TextIncomplete;
        theme.TextComplete = userTheme.TextComplete;
        theme.TextIncorrect = userTheme.TextIncorrect;
        theme.InactiveKey = userTheme.InactiveKey;
        theme.InactiveText = userTheme.InactiveText;
        theme.ActiveText = userTheme.ActiveText;
        _dbContext.UserThemes.Update(theme);
        userTheme.Gradient.Select((color, index) => new GradientColor() { Id = index, Color = color, UserThemeId = userId }).ToList()
            .ForEach(gradientColor => _dbContext.GradientColors.Add(gradientColor));
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    private bool UserExists(Guid userId)
    {
        return _dbContext.Users.Any(u => u.UserId == userId);
    }
}