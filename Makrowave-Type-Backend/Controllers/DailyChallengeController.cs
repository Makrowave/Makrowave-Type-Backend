using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Makrowave_Type_Backend.Dtos;
using Makrowave_Type_Backend.Models;
using Makrowave_Type_Backend.Models.Entities;
using Makrowave_Type_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookieOptions = Microsoft.AspNetCore.Http.CookieOptions;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DailyChallengeController : ControllerBase
{
    private readonly ITextGeneratorService _textGenerator;
    private readonly string _key;
    private readonly DatabaseContext _context;
    private string _challengeText;
    private DateTime _challengeDate;

    public DailyChallengeController
        (ITextGeneratorService textGenerator, 
            IConfiguration configuration,
            DatabaseContext context)
    {
        _textGenerator = textGenerator;
        _key = configuration["ChallengeSecret"] ?? 
               throw new InvalidOperationException("ChallengeSecret is missing in configuration");
        _context = context;
        _challengeDate = DateTime.UtcNow.Date;
        _challengeText = _textGenerator.GenerateText(true);
    }
    
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpGet("daily-challenge-start")]
    public async  Task<ActionResult<string>> GetDailyChallenge()
    {
        if (DateTime.UtcNow.Date != _challengeDate)
        {
            UpdateChallenge();
        }
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var challenge = await _context.DailyRecords
            .Where(record => record.UserId == userId && record.Date.Date == _challengeDate)
            .ToListAsync();
        if (challenge.Count != 0)
        {
            return BadRequest("The daily challenge has already been completed");
        }
        var hash = GenerateHash(_challengeText, userId.ToString());
        
        Response.Cookies.Append("challenge-key", hash , new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(5),
        });
        return Ok(_challengeText);

    }

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPost("daily-challenge-end")]
    public async Task<IActionResult> EndDailyChallenge([FromBody] ChallengeDto challenge)
    {
        if (DateTime.UtcNow.Date != _challengeDate)
        {
            UpdateChallenge();
            return BadRequest("Challenge is invalid or outdated");
        }
        var challengeKey = Request.Cookies["challenge-key"];
        if (challengeKey == null)
        {
            return BadRequest("No challenge key provided");
        }

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
        var hash = GenerateHash(_challengeText, userId.ToString());
        if (challengeKey != hash)
        {
            return BadRequest("Challenge is invalid or outdated");
        }

        _context.DailyRecords.Add(new DailyRecord()
        {
            UserId = userId,
            Date = _challengeDate,
            Time = challenge.Time,
            Score = challenge.Score,
            Accuracy = challenge.Accuracy
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("practice")]
    public ActionResult<string> GetPracticeText()
    {
        return Ok(_textGenerator.GenerateText(false));
    }

    private string GenerateHash(string text, string userId)
    {
        using SHA256 sha256 = SHA256.Create();
        var date = DateTime.UtcNow.Date.ToString("yyyymmdd");
        var toHash = date + text + _key + userId;
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash)));
    }

    private void UpdateChallenge()
    {
        _challengeDate = DateTime.UtcNow.Date;
        _challengeText = _textGenerator.GenerateText(true);
    }
    
}