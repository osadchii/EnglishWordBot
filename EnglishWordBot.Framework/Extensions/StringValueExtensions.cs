namespace EnglishWordBot.Framework.Extensions;

public static class StringValueExtensions
{
    public static string NormalizeValue(this string value)
    {
        return value.Trim().ToLower();
    }
}