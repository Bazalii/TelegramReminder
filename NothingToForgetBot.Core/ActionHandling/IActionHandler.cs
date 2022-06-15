using Telegram.Bot;
using Telegram.Bot.Types;

namespace NothingToForgetBot.Core.ActionHandling;

public interface IMessageHandler
{
    Task HandleChatUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    Task HandlePollingError(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken);
}