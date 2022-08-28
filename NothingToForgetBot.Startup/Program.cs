using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingToForgetBot.Core;
using NothingToForgetBot.Core.ChatActions.UpdateHandling;
using NothingToForgetBot.Data;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace NothingToForgetBot.Startup;

public static class Program
{
    private static IHost _host = null!;

    private static IConfiguration _configuration = null!;

    private static Task Main(string[] args)
    {
        _host = CreateHostBuilder(args).Build();

        StartBot();
        
        return _host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, builder) =>
            {
                _configuration = builder
                    .AddJsonFile("appsettings.json")
                    .Build();
            })
            .ConfigureServices((_, services) =>
                services
                    .AddSingleton<ITelegramBotClient>(_ =>
                        new TelegramBotClient(""))
                    .AddCore(_configuration)
                    .AddData(_configuration)
            );

    private static void StartBot()
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        
        _host.Services.GetRequiredService<ITelegramBotClient>().StartReceiving(
            updateHandler: _host.Services.GetRequiredService<IChatUpdateHandler>().HandleChatUpdate,
            pollingErrorHandler: _host.Services.GetRequiredService<IChatUpdateHandler>().HandlePollingError,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationTokenSource.Token
        );
    }
}