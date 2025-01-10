using Makrowave_Type_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DailyChallengeController : ControllerBase
{
    private readonly ITextGeneratorService _textGenerator;
    private readonly string _key;

    public DailyChallengeController(ITextGeneratorService textGenerator, IConfiguration configuration)
    {
        _textGenerator = textGenerator;
        _key = configuration["ChallengeSecret"] ?? throw new InvalidOperationException("ChallengeSecret is missing in configuration");

    }
    
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpGet("daily-challenge-end")]
    public ActionResult<string> GetDailyChallenge()
    {
        var text = _textGenerator.GenerateText(true);
        throw new NotImplementedException();
        
    }

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPost("daily-challenge-start")]
    public IActionResult EndDailyChallenge()
    {
        throw new NotImplementedException();
    }

    [HttpGet("practice")]
    public ActionResult<string> GetPracticeText()
    {
        return Ok(_textGenerator.GenerateText(false));
    }
}