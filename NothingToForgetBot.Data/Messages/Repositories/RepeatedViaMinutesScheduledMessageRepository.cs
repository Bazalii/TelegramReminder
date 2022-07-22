using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Messages.Models;

namespace NothingToForgetBot.Data.Messages.Repositories;

public class RepeatedViaMinutesMessageRepository : IRepeatedViaMinutesMessageRepository
{
    private readonly NothingToForgetBotContext _context;

    public RepeatedViaMinutesMessageRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(RepeatedViaMinutesMessage repeatedMessage, CancellationToken cancellationToken)
    {
        await _context.RepeatedViaMinutesMessages.AddAsync(new RepeatedViaMinutesMessageDbModel
        {
            Id = repeatedMessage.Id,
            ChatId = repeatedMessage.ChatId,
            Content = repeatedMessage.Content,
            Interval = repeatedMessage.Interval,
            EndDate = repeatedMessage.EndDate
        }, cancellationToken);
    }

    public async Task<RepeatedViaMinutesMessage> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaMinutesMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        return new RepeatedViaMinutesMessage
        {
            Id = dbModel.Id,
            ChatId = dbModel.ChatId,
            Content = dbModel.Content,
            Interval = dbModel.Interval,
            EndDate = dbModel.EndDate
        };
    }

    public async Task<List<RepeatedViaMinutesMessage>> GetAllByChatId(long chatId,
        CancellationToken cancellationToken)
    {
        return await _context.RepeatedViaMinutesMessages
            .AsNoTracking()
            .Select(message => new RepeatedViaMinutesMessage
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

    public async Task Update(RepeatedViaMinutesMessage repeatedMessage, CancellationToken cancellationToken)
    {
        var dbModel = await _context.RepeatedViaMinutesMessages
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
        var dbModel = await _context.RepeatedViaMinutesMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken);
        
        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Repeated message with id: {id} is not found!");
        }

        _context.RepeatedViaMinutesMessages.Remove(dbModel);
    }
}