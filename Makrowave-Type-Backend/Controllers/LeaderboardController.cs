using System.Security.Claims;
using Makrowave_Type_Backend.Dtos;
using Makrowave_Type_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Makrowave_Type_Backend.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly DatabaseContext _context;

    public LeaderboardController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("get-daily")]
    public async Task<ActionResult<List<DailyScoreDto>>> GetDailyLeaderboard()
    {
        var date = DateTime.Now.Date;
        var records = await _context.DailyRecords.Where(record => record.Date.Date == date)
            .Join(_context.Users, record => record.UserId, user => user.UserId, (record, user) => new
            {
                Username = user.Username,
                Score = record.Score,
                Time = record.Time,
                Accuracy = record.Accuracy
            }).OrderByDescending(score => score.Score).ToListAsync();
            var scores = records.Select((result, index) => new DailyScoreDto()
            {
                Username = result.Username,
                Score = result.Score,
                Time = result.Time,
                Accuracy = result.Accuracy,
                Place = index + 1
            })
            .ToList(); 
        
        //If user is logged in and didn't score in top 20, put them in there
        var result = scores.Take(20).ToList();
        if (Guid.TryParse(User.FindFirst(ClaimTypes.Name)?.Value, out Guid userId))
        {
            var username = (await _context.Users.FindAsync(userId))?.Username;
            var userScore = scores.FirstOrDefault(record => record.Username == username);
            if (!result.Any(score => score.Username == username) && userScore != null)
            {
                result = result.Append(userScore).ToList();
            }
        }
        return Ok(result);
    }

    [HttpGet("get-alltime")]
    public async Task<ActionResult<List<AllTimeScoreDto>>> GetAllTimeLeaderboard()
    {
        var records = await _context.DailyRecords.ToListAsync();
        var scores = records.GroupBy(record => record.Date.Date)
            .Select(group => group.OrderByDescending(record => record.Score).First())
            .Join(_context.Users, record => record.UserId, user => user.UserId,
                (record, user) => new { user.Username, record.Date.Date })
            .GroupBy(record => record.Username)
            .Select(group => new { Username = group.Key, Wins = group.Count() })
            .OrderByDescending(result => result.Wins)
            .Select((result, index) => new AllTimeScoreDto()
            {
                Username = result.Username,
                Wins = result.Wins,
                Place = index + 1
            }).ToList();
        //If user is logged in and didn't score in top 20, put them in there
        var result = scores.Take(20).ToList();
        if (Guid.TryParse(User.FindFirst(ClaimTypes.Name)?.Value, out Guid userId))
        {
            Console.WriteLine(userId);
            var username = (await _context.Users.FindAsync(userId))?.Username;
            var userScore = scores.FirstOrDefault(record => record.Username == username);
            if (!result.Any(score => score.Username == username) && userScore != null)
            {
                result = result.Append(userScore).ToList();
            }
        }
        
        return Ok(result);
    }
}