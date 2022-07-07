using Telegram.Bot;

namespace NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse.Implementations;

public class MessageSender : IMessageSender
{
    private readonly ITelegramBotClient _botClient;

    public MessageSender(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public Task SendMessageToChat(long chatId, string messageText, CancellationToken cancellationToken)
    {
        return _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: messageText,
            cancellationToken: cancellationToken);
    }
}