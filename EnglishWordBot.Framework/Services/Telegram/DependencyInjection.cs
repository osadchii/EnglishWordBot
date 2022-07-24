using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace EnglishWordBot.Framework.Services.Telegram;

public static class DependencyInjection
{
    public static void AddTelegram(this IServiceCollection serviceCollection)
    {
        // serviceCollection.AddHostedService<ConfigureWebhook>();
        // serviceCollection.AddHttpClient("tgwebhook")
        //     .AddTypedClient<ITelegramBotClient>(httpClient
        //         => new TelegramBotClient(config.BotToken!, httpClient));

        serviceCollection.AddScoped<HandleUpdateService>();
    }
}