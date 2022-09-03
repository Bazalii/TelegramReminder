using NothingToForgetBot.Core.Timers.Models;
using Timer = System.Timers.Timer;

namespace NothingToForgetBot.Core.Timers.Handlers;

public interface ITimerHandler
{
    Task Add(Guid id, Timer timer);
    Task Add(Guid id, RepeatedMessageTimers timers);
    Task RemoveScheduledMessageTimer(Guid id, CancellationToken cancellationToken);
    Task RemoveRepeatedViaMinutesMessageTimers(Guid id, CancellationToken cancellationToken);
    Task RemoveRepeatedViaSecondsMessageTimers(Guid id, CancellationToken cancellationToken);
}