using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot.Services;


//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddHostedService<BotService>();
//var app = builder.Build();

//app.MapGet("/", () => "Bot is running!");
//app.Run();

//var host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((context, services) =>
//    {
//       // Конфигурация бота
//       services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7996792416:AAFGNZkdgemeWxqT7_JMVtsJxiIWf3TyQow"));
//       // Регистрация сервисов
//       services.AddScoped<IUpdateHandler, UpdateHandler>();
//       services.AddHostedService<BotService>();

//       // Настройка логирования
//       services.AddLogging(logging =>
//       {
//          logging.ClearProviders();
//          logging.AddConsole();
//          logging.SetMinimumLevel(LogLevel.Debug);
//       });
//    })
//    .Build();

//await host.RunAsync();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<ITelegramBotClient>(_ =>
        new TelegramBotClient("7996792416:AAFGNZkdgemeWxqT7_JMVtsJxiIWf3TyQow"))
    .AddScoped<IUpdateHandler, UpdateHandler>()
    .AddHostedService<BotService>();

var app = builder.Build();
app.MapGet("/", () => "Telegram Bot is running!");
app.Run();
