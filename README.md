# Makrowave-Type-Backend
Backend for [Makrowave-Type](https://github.com/Makrowave/Makrowave-Type).
It uses PostgreSQL for database and simple session database for auth.

## Setup
appsettings.json:
Change:
"Urls" - to the url with port you want the service to be running
"AllowedOrigins" to match your CORS origins, 
"ConnectionStrings:DefaultConnection" to your psql database connection string,
"ChallengeSecret" to random generated secret for daily challenges (can be any string)
Other:
"SessionDuration" - session duration in minutes, default 1 week
"Theme" - default colors for users' themes

run
```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet publish -c Release -o /your/directory
```
