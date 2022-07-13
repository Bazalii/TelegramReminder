using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Messages.Models;

namespace NothingToForgetBot.Data.Messages.Repositories;

public class RepeatedViaSecondsMessageRepository : IRepeatedViaSecondsScheduledMessageRepository
{
    private readonly NothingToForgetBotContext _context;

    public RepeatedViaSecondsMessageRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(RepeatedViaSecondsScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        await _context.RepeatedViaSecondsMessages.AddAsync(new RepeatedViaSecondsMessageDbModel
        {
            Id = scheduledMessage.Id,
            ChatId = scheduledMessage.ChatId,
            Content = scheduledMessage.Content,
            Interval = scheduledMessage.Interval,
            EndDate = scheduledMessage.EndDate
        }, cancellationToken);
    }

    public async Task<RepeatedViaSecondsScheduledMessage> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaSecondsMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        return new RepeatedViaSecondsScheduledMessage
        {
            Id = dbModel.Id,
            ChatId = dbModel.ChatId,
            Content = dbModel.Content,
            Interval = dbModel.Interval,
            EndDate = dbModel.EndDate
        };
    }

    public async Task<List<RepeatedViaSecondsScheduledMessage>> GetAllByChatId(long chatId,
        CancellationToken cancellationToken)
    {
        return await _context.RepeatedViaSecondsMessages
            .AsNoTracking()
            .Select(message => new RepeatedViaSecondsScheduledMessage
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

    public async Task Update(RepeatedViaSecondsScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaSecondsMessages
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
        var dbModel = await _context.RepeatedViaSecondsMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        _context.RepeatedViaSecondsMessages.Remove(dbModel);
    }
}