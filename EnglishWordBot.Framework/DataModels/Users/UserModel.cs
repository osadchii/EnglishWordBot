using EnglishWordBot.Framework.DataModels.Words;

namespace EnglishWordBot.Framework.DataModels.Users;

public class UserModel
{
    public long ChatId { get; set; }
    public IEnumerable<WordModel> Words { get; set; }
}