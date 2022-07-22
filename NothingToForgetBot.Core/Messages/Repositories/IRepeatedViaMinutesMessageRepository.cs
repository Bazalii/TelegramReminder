using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IRepeatedViaMinutesMessageRepository
{
    Task Add(RepeatedViaMinutesMessage repeatedMessage, CancellationToken cancellationToken);

    Task<RepeatedViaMinutesMessage> GetById(Guid id, CancellationToken cancellationToken);

    Task<List<RepeatedViaMinutesMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken);

    Task Update(RepeatedViaMinutesMessage repeatedMessage, CancellationToken cancellationToken);

    Task Remove(Guid id, CancellationToken cancellationToken);
}