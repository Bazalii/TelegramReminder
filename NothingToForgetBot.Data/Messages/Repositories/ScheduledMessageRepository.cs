using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Messages.Models;

namespace NothingToForgetBot.Data.Messages.Repositories;

public class ScheduledMessageRepository : IScheduledMessageRepository
{
    private readonly NothingToForgetBotContext _context;

    public ScheduledMessageRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(ScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        await _context.ScheduledMessages.AddAsync(new ScheduledMessageDbModel
        {
            Id = scheduledMessage.Id,
            ChatId = scheduledMessage.ChatId,
            Content = scheduledMessage.Content,
            PublishingDate = scheduledMessage.PublishingDate
        }, cancellationToken);
    }

    public async Task<ScheduledMessage> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ScheduledMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Scheduled message with id: {id} is not found!");
        }

        return new ScheduledMessage
        {
            Id = dbModel.Id,
            ChatId = dbModel.ChatId,
            Content = dbModel.Content,
            PublishingDate = dbModel.PublishingDate,
        };
    }

    public async Task<List<ScheduledMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken)
    {
        return await _context.ScheduledMessages
            .AsNoTracking()
            .Select(message => new ScheduledMessage
            {
                Id = message.Id,
                ChatId = message.ChatId,
                Content = message.Content,
                PublishingDate = message.PublishingDate
            })
            .Where(message => message.ChatId == chatId)
            .ToListAsync(cancellationToken);
    }

    public async Task Update(ScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ScheduledMessages
            .FirstOrDefaultAsync(message => message.Id == scheduledMessage.Id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Scheduled message with id: {scheduledMessage.Id} is not found!");
        }

        dbModel.ChatId = scheduledMessage.ChatId;
        dbModel.Content = scheduledMessage.Content;
        dbModel.PublishingDate = scheduledMessage.PublishingDate;
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ScheduledMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Scheduled message with id: {id} is not found!");
        }

        _context.ScheduledMessages.Remove(dbModel);
    }
}