namespace NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;

public interface IMessageSender
{
    Task SendMessageToChat(long chatId, string messageText, CancellationToken cancellationToken);
}