using EnglishWordBot.Framework.Database;
using EnglishWordBot.Framework.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration
    .AddJsonFile("appsettings.json", true);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddWordBotServices();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

InitializeDatabase(app);

app.Run();

void InitializeDatabase(IApplicationBuilder appServices)
{
    using var scope = appServices.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
    scope?.ServiceProvider.GetRequiredService<WordContext>().Database.Migrate();
}