using Microsoft.AspNetCore.Mvc;

namespace Makrowave_Type_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DailyChallengeController
{
    [HttpGet("daily-challenge")]
    public ActionResult<string> GetDailyChallenge()
    {
        throw new NotImplementedException();
    }

    [HttpPost("daily-challenge")]
    public ActionResult<string> EndDailyChallenge()
    {
        throw new NotImplementedException();
    }
}