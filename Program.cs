using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);
var botToken = "7996792416:AAFGNZkdgemeWxqT7_JMVtsJxiIWf3TyQow";

var botClient = new TelegramBotClient(botToken);
builder.Services.AddSingleton(botClient);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    endpoints.MapPost("https://telegrambot-rx83.onrender.com/api/webhook", async context =>
    {
        using var reader = new StreamReader(context.Request.Body);
        var json = await reader.ReadToEndAsync();
        var update = JsonConvert.DeserializeObject<Update>(json);
        
        if (update?.Message is {} message)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Echo: {message.Text}");
        }
    });
});

// Set webhook on startup
app.Lifetime.ApplicationStarted.Register(async () => 
{
    var webhookUrl = $"https://telegrambot-rx83.onrender.com/api/webhook";
    await botClient.SetWebhookAsync(webhookUrl);
});

app.Run();