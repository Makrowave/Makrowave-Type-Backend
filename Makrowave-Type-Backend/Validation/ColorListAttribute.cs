using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Makrowave_Type_Backend.Validation;

public class ColorListAttribute : ValidationAttribute
{

    public string GetFormatErrorMessage()
    {
        return "One of the colors has invalid format.";
    }

    public string GetCountErrorMessage()
    {
        return "Amount of colors must be between 2 and 32767.";
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var colorList = (List<string>)validationContext.ObjectInstance;
        if (colorList.Count < 2 || colorList.Count > 32767)
        {
            return new ValidationResult(GetCountErrorMessage());
        }

        if (colorList.Any(color => !Regex.IsMatch(color, Regexes.Hex)))
        {
            return new ValidationResult(GetFormatErrorMessage());
        }
        return ValidationResult.Success;
    }   
}