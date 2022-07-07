using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IScheduledMessageRepository
{
    Task Add(ScheduledMessage scheduledMessage, CancellationToken cancellationToken);

    Task<ScheduledMessage> GetById(Guid id, CancellationToken cancellationToken);

    Task<List<ScheduledMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken);

    Task Update(ScheduledMessage scheduledMessage, CancellationToken cancellationToken);

    Task Remove(Guid id, CancellationToken cancellationToken);
}