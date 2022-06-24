namespace NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;

public interface IMessageSender
{
    public Task SendMessageToChat(long chatId, string messageText, CancellationToken cancellationToken);
}