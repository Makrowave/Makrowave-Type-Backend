namespace Makrowave_Type_Backend.Validation;

public struct Regexes
{
    public const string Name = @"^[a-zA-Z0-9_\-][a-zA-Z0-9_\- ]{0,14}[a-zA-Z0-9_\-]$";
    public const string Password = "^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[@$!%*#?&_=()^-])[A-Za-z0-9@$!%*#?&]{8,}$";
    public const string Hex = "^#[0-9A-Fa-f]{6}$";
}