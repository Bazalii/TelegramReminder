using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Repositories;

public interface IScheduledMessageRepository
{
    Task Add(ScheduledMessage scheduledMessage);

    Task<ScheduledMessage> GetById(Guid id);
    
    Task<List<ScheduledMessage>> GetAllByChatId(long chatId);
    
    Task Update(ScheduledMessage scheduledMessage);
    
    Task Remove(Guid id);
}