using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBot.Services;

public class BotService : BackgroundService
{
   private readonly ITelegramBotClient _botClient;
   private readonly IUpdateHandler _updateHandler;

   public BotService(
       ITelegramBotClient botClient,
       IUpdateHandler updateHandler)
   {
      _botClient = botClient;
      _updateHandler = updateHandler;
   }

   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
   {
      // Удаляем вебхук, если он был установлен
      await _botClient.DeleteWebhook(cancellationToken: stoppingToken);

      var me = await _botClient.GetMe(stoppingToken);
      Console.WriteLine($"Bot @{me.Username} started!");

      _botClient.StartReceiving(
          updateHandler: _updateHandler,
          receiverOptions: null,
          cancellationToken: stoppingToken
      );

      await Task.Delay(Timeout.Infinite, stoppingToken);
   }
}