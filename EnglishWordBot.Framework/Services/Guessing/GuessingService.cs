using EnglishWordBot.Framework.DataModels.Users;
using EnglishWordBot.Framework.Extensions;

namespace EnglishWordBot.Framework.Services.Guessing;

public interface IGuessingService
{
    string RandomWord(UserModel userModel);
    bool GuessTranslation(UserModel userModel, string translation);
}

public class GuessingService : IGuessingService
{
    public string RandomWord(UserModel userModel)
    {
        var words = userModel.Words
            .Where(w => !w.Disabled && !w.Learned).ToArray();
        
        if (!words.Any())
        {
            throw new Exception("User has not any words");
        }
        
        var random = new Random(DateTime.UtcNow.Millisecond);
        return words[random.Next(0, words.Length)].Value;
    }

    public bool GuessTranslation(UserModel userModel, string translation)
    {
        var word = userModel.Words
            .SingleOrDefault(w => w.Value.Equals(userModel.CurrentWord));

        if (word is null)
        {
            throw new Exception("Word not found in user model");
        }

        return word.Translations.Any(t =>
            t.Equals(translation.NormalizeValue(), StringComparison.InvariantCultureIgnoreCase));
    }
}