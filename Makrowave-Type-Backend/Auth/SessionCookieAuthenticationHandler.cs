using System.Security.Claims;
using System.Text.Encodings.Web;
using Makrowave_Type_Backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Makrowave_Type_Backend.Auth;

public class SessionCookieAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly DatabaseContext _context;
    public SessionCookieAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock,
        DatabaseContext context) : base(options, logger, encoder, clock)
    {
        _context = context;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var sessionId = Request.Cookies["session"];
        Console.WriteLine("Session id: " + sessionId);
        if (string.IsNullOrEmpty(sessionId))
        {
            Console.WriteLine("No session id found.");
            return AuthenticateResult.NoResult();
        }

        if (!Guid.TryParse(sessionId, out Guid sessionGuid))
        {
            Console.WriteLine("Invalid session GUID.");
            return AuthenticateResult.NoResult();
        }

        var session = await _context.Sessions.FindAsync(sessionGuid);
        if (session == null)
        {
            Console.WriteLine("No session found.");
            return AuthenticateResult.NoResult();
        }

        if (session.ExpirationDate < DateTime.Now)
        {
            Console.WriteLine("Session expired.");
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return AuthenticateResult.NoResult();
        }

        var user = await _context.Users.FindAsync(session.UserId);
        if (user == null)
        {
            Console.WriteLine("No user found.");
            return AuthenticateResult.NoResult();
        }
        
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, session.UserId.ToString())], "SessionCookie");
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}