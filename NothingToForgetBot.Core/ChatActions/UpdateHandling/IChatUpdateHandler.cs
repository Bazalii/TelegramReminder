using Telegram.Bot;
using Telegram.Bot.Types;

namespace NothingToForgetBot.Core.ChatActions.UpdateHandling;

public interface IChatUpdateHandler
{
    Task HandleChatUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    Task HandlePollingError(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken);
}