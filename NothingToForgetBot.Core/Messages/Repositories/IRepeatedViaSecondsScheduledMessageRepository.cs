using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IRepeatedViaSecondsScheduledMessageRepository
{
    Task Add(RepeatedViaSecondsScheduledMessage scheduledMessage);

    Task<RepeatedViaSecondsScheduledMessage> GetById(Guid id);
    
    Task<List<RepeatedViaSecondsScheduledMessage>> GetAllByChatId(long chatId);
    
    Task Update(RepeatedViaSecondsScheduledMessage scheduledMessage);
    
    Task Remove(Guid id);
}