namespace EnglishWordBot.Framework.Database.Entities.Users;

public class User : BaseEntity
{
    public long ChatId { get; set; }
    public string CurrentWord { get; set; }
    public string CurrentState { get; set; }
}