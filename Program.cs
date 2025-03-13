using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
var botToken = builder.Configuration["TelegramBot:Token"];

var botClient = new TelegramBotClient(botToken);
builder.Services.AddSingleton(botClient);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    endpoints.MapPost("/api/webhook", async context =>
    {
        using var reader = new StreamReader(context.Request.Body);
        var json = await reader.ReadToEndAsync();
        var update = JsonConvert.DeserializeObject<Update>(json);
        
        if (update?.Message is {} message)
        {
            await botClient.SendMessage(message.Chat.Id, $"Echo: {message.Text}");
        }
    });
});

// Set webhook on startup
app.Lifetime.ApplicationStarted.Register(async () => 
{
    var webhookUrl = $"{builder.Configuration["TelegramBot:WebhookUrl"]}/api/webhook";
    await botClient.SetWebhook(webhookUrl);
});
app.MapGet("/", () => "Telegram Bot is running!");
app.Run();