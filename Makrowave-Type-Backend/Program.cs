using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Makrowave_Type_Backend.Auth;
using Makrowave_Type_Backend.Models;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);


//Loading configuration
builder.Configuration.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}
builder.Configuration.AddEnvironmentVariables();

//Add database context
builder.Services.AddDbContextPool<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add controllers
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//Services
builder.Services.AddSingleton<DefaultTheme>();

//Authentication
builder.Services.AddAuthentication("SessionCookie")
    .AddScheme<AuthenticationSchemeOptions, SessionCookieAuthenticationHandler>("SessionCookie", null);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SessionCookie", policy =>
    {
        policy.AddAuthenticationSchemes("SessionCookie");
        policy.RequireAuthenticatedUser();
    });
});
//Cors
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();