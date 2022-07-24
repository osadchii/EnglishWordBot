namespace EnglishWordBot.Framework.DataModels.Words;

public class WordModel
{
    public string Value { get; set; }
    public IEnumerable<string> Translations { get; set; }
    public bool Disabled { get; set; }
    public bool Learned { get; set; }

    public bool InProcess => !Disabled && !Learned;
}