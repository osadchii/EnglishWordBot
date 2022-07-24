using EnglishWordBot.Framework.Database;
using EnglishWordBot.Framework.Database.Entities.Words;
using EnglishWordBot.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordBot.Framework.Services.Words;

public interface IWordsService
{
    Task AddWord(string value);
    Task AddTranslation(string word, string translation);
}

public class WordsService : IWordsService
{
    private readonly WordContext _context;

    public WordsService(WordContext context)
    {
        _context = context;
    }

    public async Task AddWord(string value)
    {
        var normalizedValue = value.NormalizeValue();
        var wordExists = await _context.Words
            .AsNoTracking()
            .AnyAsync(w => w.Value.Equals(normalizedValue, StringComparison.InvariantCultureIgnoreCase));

        if (wordExists)
        {
            return;
        }

        await _context.Words.AddAsync(new Word
        {
            Id = Guid.NewGuid(),
            Value = normalizedValue
        });
        await _context.SaveChangesAsync();
    }

    public async Task AddTranslation(string word, string translation)
    {
        var normalizedValue = word.NormalizeValue();
        var normalizedTranslation = word.NormalizeValue();
        var wordEntry = await _context.Words
            .SingleOrDefaultAsync(w => w.Value.Equals(normalizedValue, StringComparison.InvariantCultureIgnoreCase));

        if (wordEntry is null)
        {
            throw new Exception("Word not found");
        }
        
        wordEntry.Translations.Add(new Word.Translation
        {
            Id = Guid.NewGuid(),
            Value = normalizedTranslation
        });

        await _context.SaveChangesAsync();
    }
}