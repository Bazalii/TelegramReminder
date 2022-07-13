using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Messages.Models;

namespace NothingToForgetBot.Data.Messages.Repositories;

public class RepeatedViaMinutesMessageRepository : IRepeatedViaMinutesScheduledMessageRepository
{
    private readonly NothingToForgetBotContext _context;

    public RepeatedViaMinutesMessageRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(RepeatedViaMinutesScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        await _context.RepeatedViaMinutesMessages.AddAsync(new RepeatedViaMinutesMessageDbModel
        {
            Id = scheduledMessage.Id,
            ChatId = scheduledMessage.ChatId,
            Content = scheduledMessage.Content,
            Interval = scheduledMessage.Interval,
            EndDate = scheduledMessage.EndDate
        }, cancellationToken);
    }

    public async Task<RepeatedViaMinutesScheduledMessage> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaMinutesMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        return new RepeatedViaMinutesScheduledMessage
        {
            Id = dbModel.Id,
            ChatId = dbModel.ChatId,
            Content = dbModel.Content,
            Interval = dbModel.Interval,
            EndDate = dbModel.EndDate
        };
    }

    public async Task<List<RepeatedViaMinutesScheduledMessage>> GetAllByChatId(long chatId,
        CancellationToken cancellationToken)
    {
        return await _context.RepeatedViaMinutesMessages
            .AsNoTracking()
            .Select(message => new RepeatedViaMinutesScheduledMessage
            {
                Id = message.Id,
                ChatId = message.ChatId,
                Content = message.Content,
                Interval = message.Interval,
                EndDate = message.EndDate
            })
            .Where(message => message.ChatId == chatId)
            .ToListAsync(cancellationToken);
    }

    public async Task Update(RepeatedViaMinutesScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaMinutesMessages
            .FirstOrDefaultAsync(message => message.Id == scheduledMessage.Id, cancellationToken);
        
        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {scheduledMessage.Id} is not found!");
        }

        dbModel.ChatId = scheduledMessage.ChatId;
        dbModel.Content = scheduledMessage.Content;
        dbModel.Interval = scheduledMessage.Interval;
        dbModel.EndDate = scheduledMessage.EndDate;
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaMinutesMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);
        
        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        _context.RepeatedViaMinutesMessages.Remove(dbModel);
    }
}