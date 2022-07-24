using EnglishWordBot.Framework.Services.Telegram;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace EnglishWordBot.Api.Controllers;

public class TelegramController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
        [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }
}