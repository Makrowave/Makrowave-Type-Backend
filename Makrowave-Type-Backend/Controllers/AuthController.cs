using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Makrowave_Type_Backend.Dtos;
using Makrowave_Type_Backend.Models;
using Makrowave_Type_Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _dbContext;
    private readonly DefaultTheme _defaultTheme;
    private readonly int _sessionDuration;
    private readonly string _secret;

    public AuthController(DatabaseContext dbContext, DefaultTheme defaultTheme, IConfiguration config)
    {
        _dbContext = dbContext;
        _defaultTheme = defaultTheme;
        _secret = config["PasswordSecret"] ?? throw new InvalidOperationException("Missing or invalid \"PasswordSecret\" in appsettings.json");
        _sessionDuration = Int32.Parse(config["SessionDuration"] 
                                       ?? throw new InvalidOperationException("Missing or invalid \"SessionDuration\" in appsettings.json"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var username = loginDto.Username;
        var password = loginDto.Password;
        if (!UserExists(username)) return Unauthorized("Incorrect username or password");
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (!Argon2.Verify(user!.PasswordHash, password))
        {
            return Unauthorized("Incorrect username or password");
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
    public async Task<IActionResult> Register([FromBody] RegisterDto user)
    {
        var username = user.Username;
        var password = user.Password;
        if (!ModelState.IsValid)
        {
            return BadRequest("Data didn't pass validation");
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
        _dbContext.Sessions.Remove(session!);
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

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPut("change-username")]
    public async Task<IActionResult> ChangeUsername([FromBody] UsernameDto usernameDto)
    {
        var username = usernameDto.Username;
        //Check for new username
        if (UserExists(username))
        {
            return BadRequest("User with that username already exists");
        }
        //Change username
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var user = await _dbContext.Users.FindAsync(userId);
        user!.Username = username;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        //Logout
        var sessionId = Guid.Parse(User.FindFirst(ClaimTypes.Authentication)!.Value);
        var session = _dbContext.Sessions.Find(sessionId);
        _dbContext.Sessions.Remove(session!);
        Response.Cookies.Delete("session");
        return Ok();
    }
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordDto passwordDto)
    {
        var password = passwordDto.Password;
        //change password
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var hash = Argon2.Hash(password: password);
        var user = _dbContext.Users.Find(userId)!;
        user.PasswordHash = hash;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        //Logout   
        var sessionId = Guid.Parse(User.FindFirst(ClaimTypes.Authentication)!.Value);
        var session = _dbContext.Sessions.Find(sessionId);
        _dbContext.Sessions.Remove(session!);
        Response.Cookies.Delete("session");
        return Ok();
    }
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        //Check token
        var token = Request.Cookies["delete-token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Invalid delete token");
        }
        using SHA256 sha256 = SHA256.Create();
        var toHash = userId + _secret;
        var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash)));
        if (hash != token)
        {
            return  Unauthorized("Invalid delete token");
        }
        
        //Delete account
        var user = _dbContext.Users.Find(userId)!;
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        Response.Cookies.Delete("session");
        Response.Cookies.Delete("delete-token");
        return Ok();
    }
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPost("delete-token")]
    public async Task<IActionResult> GetDeleteToken([FromBody] PasswordDto passwordDto)
    {
        var password = passwordDto.Password;
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var passwordHash = (await _dbContext.Users.FindAsync(userId))!.PasswordHash;
        if (!Argon2.Verify(passwordHash, password))
        {
            return Unauthorized("Invalid password");
        }
        //Issue token
        
        using SHA256 sha256 = SHA256.Create();
        var toHash = userId + _secret;
        var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash)));
        
        Response.Cookies.Append("delete-token", hash, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddSeconds(30),
        });
        return Ok();
    }

    private bool UserExists(string username)
    {
        return _dbContext.Users.Any(u => u.Username == username);
    }
}
