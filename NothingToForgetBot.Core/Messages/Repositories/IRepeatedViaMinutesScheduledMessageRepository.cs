using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IRepeatedViaMinutesScheduledMessageRepository
{
    Task Add(RepeatedViaMinutesScheduledMessage scheduledMessage, CancellationToken cancellationToken);

    Task<RepeatedViaMinutesScheduledMessage> GetById(Guid id, CancellationToken cancellationToken);

    Task<List<RepeatedViaMinutesScheduledMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken);

    Task Update(RepeatedViaMinutesScheduledMessage scheduledMessage, CancellationToken cancellationToken);

    Task Remove(Guid id, CancellationToken cancellationToken);
}