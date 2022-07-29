using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Exceptions;
using NothingToForgetBot.Core.TimeZones.Models;
using NothingToForgetBot.Core.TimeZones.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.TimeZones.Models;

namespace NothingToForgetBot.Data.TimeZones.Repositories;

public class TimeZoneRepository : ITimeZoneRepository
{
    private readonly NothingToForgetBotContext _context;

    public TimeZoneRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(ChatTimeZone chatTimeZone, CancellationToken cancellationToken)
    {
        var chatTimeZoneDbModel = await _context.ChatTimeZones
            .AsNoTracking()
            .FirstOrDefaultAsync(dbModel => dbModel.ChatId == chatTimeZone.ChatId, cancellationToken);

        if (chatTimeZoneDbModel is not null)
        {
            throw new ObjectAlreadyExistsException($"TimeZone for chat with id:{chatTimeZone.ChatId} is already set!");
        }

        await _context.ChatTimeZones.AddAsync(new ChatTimeZoneDbModel
        {
            ChatId = chatTimeZone.ChatId,
            TimeZone = chatTimeZone.TimeZone
        }, cancellationToken);
    }

    public async Task<ChatTimeZone> GetByChatId(long chatId, CancellationToken cancellationToken)
    {
        var chatTimeZone = await _context.ChatTimeZones
            .AsNoTracking()
            .FirstOrDefaultAsync(dbModel => dbModel.ChatId == chatId, cancellationToken);

        if (chatTimeZone is null)
        {
            return new ChatTimeZone
            {
                ChatId = chatId,
                TimeZone = 0
            };
        }

        return new ChatTimeZone
        {
            ChatId = chatTimeZone.ChatId,
            TimeZone = chatTimeZone.TimeZone
        };
    }

    public async Task Update(ChatTimeZone chatTimeZone, CancellationToken cancellationToken)
    {
        var chatTimeZoneDbModel = await _context.ChatTimeZones
            .FirstOrDefaultAsync(dbModel => dbModel.ChatId == chatTimeZone.ChatId, cancellationToken);

        if (chatTimeZoneDbModel is null)
        {
            throw new ObjectNotFoundException($"TimeZone for chat with id: {chatTimeZone.ChatId} is not found!");
        }

        chatTimeZoneDbModel.ChatId = chatTimeZone.ChatId;
        chatTimeZoneDbModel.TimeZone = chatTimeZone.TimeZone;
    }

    public async Task RemoveByChatId(long chatId, CancellationToken cancellationToken)
    {
        var chatTimeZone =
            await _context.ChatTimeZones.FirstOrDefaultAsync(dbModel => dbModel.ChatId == chatId, cancellationToken);

        if (chatTimeZone is null)
        {
            throw new ObjectNotFoundException($"TimeZone for chat with id: {chatId} is not found!");
        }

        _context.ChatTimeZones.Remove(chatTimeZone);
    }
}