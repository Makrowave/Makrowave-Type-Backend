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
        //Session in cookie not found
        if (string.IsNullOrEmpty(sessionId))
        {
            return AuthenticateResult.NoResult();
        }
        //Invalid session GUID
        if (!Guid.TryParse(sessionId, out Guid sessionGuid))
        {
            return AuthenticateResult.NoResult();
        }
        //Session in database not found
        var session = await _context.Sessions.FindAsync(sessionGuid);
        if (session == null)
        {
            return AuthenticateResult.NoResult();
        }
        //Session expired
        if (session.ExpirationDate < DateTime.UtcNow)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return AuthenticateResult.NoResult();
        }
        //User tied to session doesn't exist
        var user = await _context.Users.FindAsync(session.UserId);
        if (user == null)
        {
            return AuthenticateResult.NoResult();
        }
    
        var identity = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, session.UserId.ToString()),
            new Claim(ClaimTypes.Authentication, session.SessionId.ToString())
        ], "SessionCookie");
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}