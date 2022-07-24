using EnglishWordBot.Framework.Services.Guessing;
using EnglishWordBot.Framework.Services.Users;
using EnglishWordBot.Framework.Services.UserWordMappings;
using EnglishWordBot.Framework.Services.Words;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishWordBot.Framework.Services;

public static class DependencyInjection
{
    public static void AddWordBotServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IGuessingService, GuessingService>();
        serviceCollection.AddTransient<IUserService, UsersService>();
        serviceCollection.AddTransient<IUserWordMappingsService, UserWordMappingsService>();
        serviceCollection.AddTransient<IWordsService, WordsService>();
    }
}