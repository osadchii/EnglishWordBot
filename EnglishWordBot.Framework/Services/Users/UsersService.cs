using EnglishWordBot.Framework.Database;
using EnglishWordBot.Framework.Database.Entities.Users;
using EnglishWordBot.Framework.DataModels.Users;
using EnglishWordBot.Framework.DataModels.Words;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordBot.Framework.Services.Users;

public interface IUserService
{
    Task<UserModel> GetUser(long chatId);
}

public class UsersService : IUserService
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