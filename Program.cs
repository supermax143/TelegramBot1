using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
   private static ITelegramBotClient? botClient;

   static async Task Main(string[] args)
   {
      // Инициализация бота с токеном
      botClient = new TelegramBotClient("7996792416:AAFGNZkdgemeWxqT7_JMVtsJxiIWf3TyQow");

      // Запуск обработки входящих сообщений
      using var cts = new CancellationTokenSource();

      var receiverOptions = new ReceiverOptions
      {
         AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
      };

      botClient.StartReceiving(
          updateHandler: HandleUpdateAsync,
          errorHandler: HandleErrorAsync,
          receiverOptions: receiverOptions,
          cancellationToken: cts.Token
      );

      var me = await botClient.GetMe();
      Console.WriteLine($"Бот {me.Username} запущен и ожидает сообщений...");

      // Ожидание завершения работы
      //Console.ReadLine();
      //cts.Cancel();
      await Task.Delay(-1);
   }

   // Обработка входящих сообщений
   private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
   {
      
      if(update.Message == null)
      {
         return;
      }

      //
      //Проверяем, что сообщение содержит текст
      if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
      {
         var chatId = update.Message.Chat.Id;
         var text = update.Message.Text;

         Console.WriteLine($"Получено сообщение: {text} от {update.Message.Chat.Username}");

         // Обработка команды /start
         if (text == "/start")
         {
            await botClient.SendMessage(
                chatId: chatId,
                text: "Привет! Я ваш бот. Чем могу помочь?",
                cancellationToken: cancellationToken
            );
         }
         else
         {
            await botClient.SendMessage(
                chatId: chatId,
                text: "Вы сказали: " + text,
                cancellationToken: cancellationToken
            );
         }
      }
   }

   // Обработка ошибок
   private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
   {
      var errorMessage = exception switch
      {
         ApiRequestException apiRequestException
             => $"Ошибка API Telegram: {apiRequestException.ErrorCode}\n{apiRequestException.Message}",
         _ => exception.ToString()
      };

      Console.WriteLine(errorMessage);
      return Task.CompletedTask;
   }
}