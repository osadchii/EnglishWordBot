using EnglishWordBot.Framework.DataModels.Users;
using EnglishWordBot.Framework.Services.Guessing;
using EnglishWordBot.Framework.Services.Users;
using EnglishWordBot.Framework.Services.UserWordMappings;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EnglishWordBot.Framework.Services.Telegram;

public class HandleUpdateService
{
    private readonly IUserService _userService;
    private readonly IUserWordMappingsService _mappingsService;
    private readonly IGuessingService _guessingService;

    public HandleUpdateService(IUserService userService, IUserWordMappingsService mappingsService, IGuessingService guessingService)
    {
        _userService = userService;
        _mappingsService = mappingsService;
        _guessingService = guessingService;
    }

    public async Task EchoAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message),
            _ => null
        };

        try
        {
            if (handler is null)
            {
                return;
            }
            
            await handler;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    private async Task BotOnMessageReceived(Message message)
    {
        var user = await _userService.GetUser(message.Chat.Id);

        if (message.Text is "Add new word")
        {
            await SetAddWordState(user);
        }
        
    }

    private async Task SetAddWordState(UserModel user)
    {
        
    }
}