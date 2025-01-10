using System.Security.Claims;
using Isopoh.Cryptography.Argon2;
using Makrowave_Type_Backend.Dtos;
using Makrowave_Type_Backend.Models;
using Makrowave_Type_Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _dbContext;
    private readonly DefaultTheme _defaultTheme;
    private readonly int _sessionDuration;

    public AuthController(DatabaseContext dbContext, DefaultTheme defaultTheme, IConfiguration config)
    {
        _dbContext = dbContext;
        _defaultTheme = defaultTheme;
        _sessionDuration = Int32.Parse(config["SessionDuration"] ?? throw new InvalidOperationException("Missing or invalid \"SessionDuration\" in appsettings.json"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDto authDto)
    {
        var username = authDto.Username;
        var password = authDto.Password;
        if (!UserExists(username)) return Unauthorized();
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (!Argon2.Verify(user!.PasswordHash, password))
        {
            return Unauthorized();
        }

        var expirationDate = DateTime.UtcNow.AddMinutes(_sessionDuration);
        var session = new Session() { UserId = user.UserId, ExpirationDate = expirationDate };
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();
        Response.Cookies.Append("session", session.SessionId.ToString(), new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expirationDate
        });
        return Ok(user.Username);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDto user)
    {
        var username = user.Username;
        var password = user.Password;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (UserExists(username))
        {
            return BadRequest("User already exists");
        }
        var newUser = new User { Username = username, PasswordHash = Argon2.Hash(password) };
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        var userTheme = _defaultTheme.GenerateDefaultUserTheme(newUser.UserId);
        _dbContext.UserThemes.Add(userTheme);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionId = Guid.Parse(User.FindFirst(ClaimTypes.Authentication)!.Value);
        var session = _dbContext.Sessions.Find(sessionId);
        _dbContext.Sessions.Remove(session);
        await _dbContext.SaveChangesAsync();
        Response.Cookies.Delete("session");
        return Ok();
    }

    [HttpGet("check-session")]
    public async Task<ActionResult<bool>> CheckSession()
    {
        if (!Request.Cookies.TryGetValue("session", out var sessionId) ||
            !Guid.TryParse(sessionId, out var sessionGuid)) return Ok(false);
        var session = await _dbContext.Sessions.FindAsync(sessionGuid);
        if (session == null) return Ok(false);
        if (session.ExpirationDate > DateTime.UtcNow)
        {
            return Ok(true);
        }
        _dbContext.Sessions.Remove(session);
        Response.Cookies.Delete("session");
        await _dbContext.SaveChangesAsync();
        return Ok(false);
    }

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpGet("get-profile")]
    public async Task<ActionResult<string>> GetProfile()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var user = await _dbContext.Users.FindAsync(userId);
        return Ok(user?.Username);
    }

    bool UserExists(string username)
    {
        return _dbContext.Users.Any(u => u.Username == username);
    }
}
