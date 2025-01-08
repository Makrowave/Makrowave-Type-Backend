using Makrowave_Type_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DailyChallengeController : ControllerBase
{
    private readonly ITextGeneratorService _textGenerator;

    public DailyChallengeController(ITextGeneratorService textGenerator)
    {
        _textGenerator = textGenerator;
    }
    
    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpGet("daily-challenge-end")]
    public ActionResult<string> GetDailyChallenge()
    {
        throw new NotImplementedException();
    }

    [Authorize(Policy = "SessionCookie", AuthenticationSchemes = "SessionCookie")]
    [HttpPost("daily-challenge-start")]
    public ActionResult<string> EndDailyChallenge()
    {
        throw new NotImplementedException();
    }

    [HttpGet("practice")]
    public ActionResult<string> GetPracticeText()
    {
        return Ok(_textGenerator.GenerateText());
    }
}