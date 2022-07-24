using EnglishWordBot.Framework.Database;
using EnglishWordBot.Framework.Database.Entities.Users;
using EnglishWordBot.Framework.DataModels.Users;
using EnglishWordBot.Framework.DataModels.Words;
using EnglishWordBot.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordBot.Framework.Services.Users;

public interface IUserService
{
    Task<UserModel> GetUser(long chatId);
    Task SetCurrentWord(long chatId, string word);
    Task SetCurrentState(long chatId, string state);
}

internal class UsersService : IUserService
{
    private readonly WordContext _context;

    public UsersService(WordContext context)
    {
        _context = context;
    }

    public async Task<UserModel> GetUser(long chatId)
    {
        await CreateUserIfNotExists(chatId);

        var user = await _context.Users
            .AsNoTracking()
            .SingleAsync(u => u.ChatId == chatId);

        var mappings = await _context.UserWordMappings
            .AsNoTracking()
            .Where(m => m.UserId == user.Id)
            .Include(m => m.Word)
            .ThenInclude(w => w.Translations)
            .ToListAsync();

        var result = new UserModel
        {
            ChatId = chatId,
            CurrentWord = user.CurrentWord,
            CurrentState = user.CurrentState,
            Words = mappings.Select(m => new WordModel
            {
                Value = m.Word.Value,
                Translations = m.Word.Translations.Select(t => t.Value),
                Disabled = m.Disabled,
                Learned = m.Learned
            })
        };

        return result;
    }

    public async Task SetCurrentWord(long chatId, string word)
    {
        var user = await _context.Users
            .SingleAsync(u => u.ChatId == chatId);

        if (!string.IsNullOrEmpty(word))
        {
            var normalizedWord = word.NormalizeValue();

            var wordEntry = await _context.Words
                .AsNoTracking()
                .SingleOrDefaultAsync(w =>
                    w.Value.Equals(normalizedWord, StringComparison.InvariantCultureIgnoreCase));

            if (wordEntry is null)
            {
                throw new Exception("Word not found");
            }

            var mappingExists = await _context.UserWordMappings
                .AsNoTracking()
                .AnyAsync(m => m.UserId == user.Id && m.WordId == wordEntry.Id);

            if (!mappingExists)
            {
                throw new Exception("Word is not mapped to user");
            }

            user.CurrentWord = normalizedWord;
        }
        else
        {
            user.CurrentWord = string.Empty;
        }

        await _context.SaveChangesAsync();
    }

    public async Task SetCurrentState(long chatId, string state)
    {
        var user = await _context.Users
            .SingleAsync(u => u.ChatId == chatId);

        user.CurrentState = state;
        await _context.SaveChangesAsync();
    }

    private async Task CreateUserIfNotExists(long chatId)
    {
        var userExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.ChatId == chatId);

        if (userExists)
        {
            return;
        }

        await _context.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            ChatId = chatId
        });
        await _context.SaveChangesAsync();
    }
}