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
    public async Task<ActionResult> GetDailyLeaderboard()
    {
        var date = DateTime.Now.Date;
        var scores = await _context.DailyRecords.Where(record => record.Date.Date == date)
            .Join(_context.Users, record => record.UserId, user => user.UserId, (record, user) => new DailyScoreDto()
            {
                Username = user.Username,
                Score = record.Score,
                Time = record.Time,
                Accuracy = record.Accuracy
            }).OrderByDescending(score => score.Score).ToListAsync();
        return Ok(scores.Take(20));
    }

    [HttpGet("get-alltime")]
    public async Task<ActionResult> GetAllTimeLeaderboard()
    {
        var records = await _context.DailyRecords.ToListAsync();
        var scores = records.GroupBy(record => record.Date.Date)
            .Select(group => group.OrderByDescending(record => record.Score).First())
            .Join(_context.Users, record => record.UserId, user => user.UserId,
                (record, user) => new { user.Username, record.Date.Date })
            .GroupBy(record => record.Username)
            .Select(group => new AllTimeScoreDto()
                { Username = group.Key, Wins = group.Count() }) //Try to list before this.
            .OrderByDescending(result => result.Wins);
        return Ok(scores.Take(20));
    }
}