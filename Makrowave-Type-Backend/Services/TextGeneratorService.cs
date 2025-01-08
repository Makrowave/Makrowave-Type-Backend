namespace Makrowave_Type_Backend.Services;

public class TextGeneratorService : ITextGeneratorService
{
    public string GenerateText()
    {
        var words = File.ReadAllLines("Files/google-10000-english-no-swears.txt");
        var length = words.Length;
        var random = new Random();
        var textLenght = random.Next(20, 40);
        var challenge = string.Empty;
        for (var i = 0; i < textLenght; i++)
        {
            challenge += words[random.Next(words.Length)] += " ";
        }
        return challenge.TrimEnd();
    }
}