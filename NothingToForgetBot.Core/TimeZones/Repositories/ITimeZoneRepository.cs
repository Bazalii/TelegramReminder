using NothingToForgetBot.Core.TimeZones.Models;

namespace NothingToForgetBot.Core.TimeZones.Repositories;

public interface ITimeZoneRepository
{
    Task Add(ChatTimeZone chatTimeZone, CancellationToken cancellationToken);

    Task<ChatTimeZone> GetByChatId(long chatId, CancellationToken cancellationToken);

    Task Update(ChatTimeZone chatTimeZone, CancellationToken cancellationToken);

    Task RemoveByChatId(long chatId, CancellationToken cancellationToken);
}