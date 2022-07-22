using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Messages.Models;

namespace NothingToForgetBot.Data.Messages.Repositories;

public class RepeatedViaSecondsMessageRepository : IRepeatedViaSecondsMessageRepository
{
    private readonly NothingToForgetBotContext _context;

    public RepeatedViaSecondsMessageRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(RepeatedViaSecondsMessage repeatedMessage, CancellationToken cancellationToken)
    {
        await _context.RepeatedViaSecondsMessages.AddAsync(new RepeatedViaSecondsMessageDbModel
        {
            Id = repeatedMessage.Id,
            ChatId = repeatedMessage.ChatId,
            Content = repeatedMessage.Content,
            Interval = repeatedMessage.Interval,
            EndDate = repeatedMessage.EndDate
        }, cancellationToken);
    }

    public async Task<RepeatedViaSecondsMessage> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaSecondsMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        return new RepeatedViaSecondsMessage
        {
            Id = dbModel.Id,
            ChatId = dbModel.ChatId,
            Content = dbModel.Content,
            Interval = dbModel.Interval,
            EndDate = dbModel.EndDate
        };
    }

    public async Task<List<RepeatedViaSecondsMessage>> GetAllByChatId(long chatId, CancellationToken cancellationToken)
    {
        return await _context.RepeatedViaSecondsMessages
            .AsNoTracking()
            .Select(message => new RepeatedViaSecondsMessage
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

    public async Task Update(RepeatedViaSecondsMessage repeatedMessage, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaSecondsMessages
            .FirstOrDefaultAsync(message => message.Id == repeatedMessage.Id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {repeatedMessage.Id} is not found!");
        }

        dbModel.ChatId = repeatedMessage.ChatId;
        dbModel.Content = repeatedMessage.Content;
        dbModel.Interval = repeatedMessage.Interval;
        dbModel.EndDate = repeatedMessage.EndDate;
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