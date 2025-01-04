using Isopoh.Cryptography.Argon2;
using Makrowave_Type_Backend.Models;
using Makrowave_Type_Backend.Models.Entities;
using Makrowave_Type_Backend.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Makrowave_Type_Backend.Controllers;


[ApiController]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _dbContext;
    private readonly DefaultTheme _defaultTheme;
    private readonly int _sessionLength;

    public AuthController(DatabaseContext dbContext, DefaultTheme defaultTheme, IConfiguration config)
    {
        _dbContext = dbContext;
        _defaultTheme = defaultTheme;
        _sessionLength = Int32.Parse(config["SessionLength"] ?? throw new InvalidOperationException("Missing or invalid 'SessionLength'"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        if(!UserExists(username)) return Unauthorized();
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (!Argon2.Verify(user!.PasswordHash, password))
        {
            return Unauthorized();
        }

        var expirationDate = DateTime.UtcNow.AddMinutes(_sessionLength);
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
        return Ok(session.SessionId.ToString());
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (!RegexValidator.isValidUsername(username) || !RegexValidator.isValidPassword(password))
        {
            return BadRequest("Invalid username or password");
        }

        if (UserExists(username))
        {
            return BadRequest("User already exists");
        }
        var newUser = new User { Username = username, PasswordHash = Argon2.Hash(password)};
        await _dbContext.SaveChangesAsync();
        var userTheme = _defaultTheme.GenerateDefaultUserTheme(newUser.UserId);
        _dbContext.UserThemes.Add(userTheme);
        await _dbContext.SaveChangesAsync();
        return Ok(newUser);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string sessionId)
    {
        try
        {
            var sessionGuid = Guid.Parse(sessionId);
        }
        catch
        {
            return BadRequest("Invalid sessionId format");
        }

        var session = _dbContext.Sessions.Find(sessionId);
        if (session == null)
        {
            return BadRequest("Invalid sessionId");
        }
        _dbContext.Sessions.Remove(session);
        await _dbContext.SaveChangesAsync();
        Response.Cookies.Delete("session");
        return Ok();
    }

    bool UserExists(string username)
    {
        return _dbContext.Users.Any(u => u.Username == username);
    }
}
