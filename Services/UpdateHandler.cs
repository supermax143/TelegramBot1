using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Services;

public class UpdateHandler : IUpdateHandler
{
   private readonly ITelegramBotClient _botClient;
   private readonly ILogger<UpdateHandler> _logger;

   public UpdateHandler(
       ITelegramBotClient botClient,
       ILogger<UpdateHandler> logger)
   {
      _botClient = botClient;
      _logger = logger;
   }

   public async Task HandleUpdateAsync(
       ITelegramBotClient botClient,
       Update update,
       CancellationToken cancellationToken)
   {
      try
      {
         await HandleMessageAsync(update.Message!, cancellationToken);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Error handling update");
      }
   }

   private async Task HandleMessageAsync(
       Message message,
       CancellationToken cancellationToken)
   {
      _logger.LogInformation(
          "Received message from {Username}: {Text}",
          message.From?.Username,
          message.Text);

      await _botClient.SendMessage(
          chatId: message.Chat.Id,
          text: $"You said: {message.Text}",
          cancellationToken: cancellationToken);
   }

   public Task HandlePollingErrorAsync(
       ITelegramBotClient botClient,
       Exception exception,
       CancellationToken cancellationToken)
   {
      _logger.LogError(exception, "Polling error occurred");
      return Task.CompletedTask;
   }

   public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
   {
      _logger.LogError(exception, "Polling error occurred");
      return Task.CompletedTask;
   }
}