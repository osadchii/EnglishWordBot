using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishWordBot.Framework.Database;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        serviceCollection.AddDbContext<WordContext>((_, options) =>
        {
            options.UseNpgsql(connectionString,
                builder => { builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
        });
    }
}