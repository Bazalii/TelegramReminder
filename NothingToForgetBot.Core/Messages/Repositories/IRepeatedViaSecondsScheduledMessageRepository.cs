using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IRepeatedViaSecondsScheduledMessageRepository
{
    Task Add(RepeatedViaSecondsScheduledMessage scheduledMessage, CancellationToken cancellationToken);

    Task<RepeatedViaSecondsScheduledMessage> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<List<RepeatedViaSecondsScheduledMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken);
    
    Task Update(RepeatedViaSecondsScheduledMessage scheduledMessage, CancellationToken cancellationToken);
    
    Task Remove(Guid id, CancellationToken cancellationToken);
}