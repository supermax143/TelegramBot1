using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
       // Конфигурация бота
       services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7996792416:AAFGNZkdgemeWxqT7_JMVtsJxiIWf3TyQow"));
       // Регистрация сервисов
       services.AddScoped<IUpdateHandler, UpdateHandler>();
       services.AddHostedService<BotService>();

       // Настройка логирования
       services.AddLogging(logging =>
       {
          logging.ClearProviders();
          logging.AddConsole();
          logging.SetMinimumLevel(LogLevel.Debug);
       });
    })
    .Build();

await host.RunAsync();
//await Task.Delay(-1);