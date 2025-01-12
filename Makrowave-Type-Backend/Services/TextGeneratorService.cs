using System.Security.Cryptography;
using System.Text;

namespace Makrowave_Type_Backend.Services;

public class TextGeneratorService : ITextGeneratorService
{
    private readonly string _secret;

    public TextGeneratorService(IConfiguration configuration)
    {
        _secret = configuration["ChallengeSecret"] 
                  ?? throw new InvalidOperationException("ChallengeSecret not found in configuration");
    }
    
    public string GenerateText(bool daily)
    {
        var words = File.ReadAllLines("Files/google-10000-english-no-swears.txt");
        var length = words.Length;
        Random random;
        if (daily)
            random = new Random(GetSeed());
        else
            random = new Random();
        var textLenght = random.Next(20, 30);
        var challenge = string.Empty;
        for (var i = 0; i < textLenght; i++)
        {
            challenge += words[random.Next(words.Length)] += " ";
        }
        return challenge.TrimEnd();
    }
    private int GetSeed()
    {
        var date = DateTime.UtcNow.Date;
        var toHash = date.ToString("yyyyMMdd") + _secret;
        using SHA256 sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash));
        return BitConverter.ToInt32(hash, 0);
    }
}