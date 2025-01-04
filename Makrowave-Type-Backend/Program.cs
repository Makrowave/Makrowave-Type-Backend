using Makrowave_Type_Backend.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
}
else
{
    builder.Configuration.SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
}

var connectionString = builder.Configuration["ConnectionStrings.Database"];

//Add database context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DefaultTheme>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();