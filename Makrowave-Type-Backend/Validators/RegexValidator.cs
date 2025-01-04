using System.Text.RegularExpressions;

namespace Makrowave_Type_Backend.Validators;

public static class RegexValidator
{
    //At least 2 characters A-Z a-z 0-9 - _ and space with no trailing whitespace
    public static bool isValidUsername(string username)
    {
        return Regex.IsMatch(username, @"^[a-zA-Z0-9_\-][a-zA-Z0-9_\- ]{0,14}[a-zA-Z0-9_\-]$");
    }

    //At least 8 characters, must have number, lower case, upper case and special char
    public static bool isValidPassword(string password)
    {
        return Regex.IsMatch(password, "^(?=.*[A-Za-z])(?=.*0-9)(?=.*[@$!%*#?&_=()^-])[A-Za-z0-9@$!%*#?&]{8,}$");
    }

    public static bool isValidHex(string hex)
    {
        return Regex.IsMatch(hex, "^#[0-9A-F]{6}$");
    }
}