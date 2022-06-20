using Message = NothingToForgetBot.Core.Messages.Models.Message;

namespace NothingToForgetBot.Core.Messages.Handlers;

public interface IScheduledMessageHandler
{
    Task Handle(Message message, CancellationToken cancellationToken);
}