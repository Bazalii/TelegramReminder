using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IRepeatedViaSecondsMessageRepository
{
    Task Add(RepeatedViaSecondsMessage repeatedMessage, CancellationToken cancellationToken);
    Task<RepeatedViaSecondsMessage> GetById(Guid id, CancellationToken cancellationToken);
    Task<List<RepeatedViaSecondsMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken);
    Task Update(RepeatedViaSecondsMessage repeatedMessage, CancellationToken cancellationToken);
    Task Remove(Guid id, CancellationToken cancellationToken);
}