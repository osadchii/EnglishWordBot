using EnglishWordBot.Framework.Database;
using EnglishWordBot.Framework.Database.Entities.Users;
using EnglishWordBot.Framework.Database.Entities.UserWordMappings;
using EnglishWordBot.Framework.Database.Entities.Words;
using EnglishWordBot.Framework.Extensions;
using EnglishWordBot.Framework.Services.Words;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordBot.Framework.Services.UserWordMappings;

public interface IUserWordMappingsService
{
    Task AddWord(long chatId, string word);
    Task DisableWord(long chatId, string word);
    Task LearnWord(long chatId, string word);
    Task IncreaseSuccess(long chatId, string word);
    Task IncreaseFail(long chatId, string word);
}

internal class UserWordMappingsService : IUserWordMappingsService
{
    private readonly IWordsService _wordsService;
    private readonly WordContext _context;

    public UserWordMappingsService(IWordsService wordsService, WordContext context)
    {
        _wordsService = wordsService;
        _context = context;
    }

    public async Task AddWord(long chatId, string word)
    {
        await _wordsService.AddWord(word);
        var (user, wordEntry) = await GetUserWord(chatId, word);

        var mappingExists = await _context.UserWordMappings
            .AsNoTracking()
            .AnyAsync(m => m.UserId == user.Id && m.WordId == wordEntry.Id);

        if (mappingExists)
        {
            return;
        }

        await _context.UserWordMappings
            .AddAsync(new UserWordMapping
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                WordId = wordEntry.Id
            });

        await _context.SaveChangesAsync();
    }

    public async Task DisableWord(long chatId, string word)
    {
        var (user, wordEntry) = await GetUserWord(chatId, word);

        var mapping = await _context.UserWordMappings
            .SingleOrDefaultAsync(m => m.UserId == user.Id && m.WordId == wordEntry.Id);

        if (mapping is null)
        {
            throw new Exception("Mapping not found");
        }

        mapping.Disabled = true;
        await _context.SaveChangesAsync();
    }

    public async Task LearnWord(long chatId, string word)
    {
        var (user, wordEntry) = await GetUserWord(chatId, word);

        var mapping = await _context.UserWordMappings
            .SingleOrDefaultAsync(m => m.UserId == user.Id && m.WordId == wordEntry.Id);

        if (mapping is null)
        {
            throw new Exception("Mapping not found");
        }

        mapping.Learned = true;
        await _context.SaveChangesAsync();
    }

    public async Task IncreaseSuccess(long chatId, string word)
    {
        var (user, wordEntry) = await GetUserWord(chatId, word);

        var mapping = await _context.UserWordMappings
            .SingleOrDefaultAsync(m => m.UserId == user.Id && m.WordId == wordEntry.Id);

        if (mapping is null)
        {
            throw new Exception("Mapping not found");
        }

        mapping.SuccessCount++;
        await _context.SaveChangesAsync();
    }

    public async Task IncreaseFail(long chatId, string word)
    {
        var (user, wordEntry) = await GetUserWord(chatId, word);

        var mapping = await _context.UserWordMappings
            .SingleOrDefaultAsync(m => m.UserId == user.Id && m.WordId == wordEntry.Id);

        if (mapping is null)
        {
            throw new Exception("Mapping not found");
        }

        mapping.FailCount++;
        await _context.SaveChangesAsync();
    }

    private async Task<(User user, Word word)> GetUserWord(long chatId, string wordValue)
    {
        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.ChatId == chatId);

        if (user is null)
        {
            throw new Exception("User not found");
        }

        var wordEntry = await _context.Words
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.Value.Equals(wordValue.NormalizeValue(),
                StringComparison.InvariantCultureIgnoreCase));

        if (wordEntry is null)
        {
            throw new Exception("Word not found");
        }

        return (user, wordEntry);
    }
}