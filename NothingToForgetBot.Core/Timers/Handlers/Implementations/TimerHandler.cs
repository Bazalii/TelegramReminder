using NothingToForgetBot.Core.Exceptions;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Core.Timers.Models;
using Timer = System.Timers.Timer;

namespace NothingToForgetBot.Core.Timers.Handlers.Implementations;

public class TimerHandler : ITimerHandler
{
    private readonly Dictionary<Guid, Timer> _scheduledMessageTimers = new();

    private readonly Dictionary<Guid, RepeatedMessageTimers> _repeatedMessageTimers = new();

    private readonly IScheduledMessageRepository _scheduledMessageRepository;

    private readonly IRepeatedViaMinutesMessageRepository _repeatedViaMinutesMessageRepository;

    private readonly IRepeatedViaSecondsMessageRepository _repeatedViaSecondsMessageRepository;

    private readonly IUnitOfWork _unitOfWork;

    public TimerHandler(IScheduledMessageRepository scheduledMessageRepository,
        IRepeatedViaMinutesMessageRepository repeatedViaMinutesMessageRepository,
        IRepeatedViaSecondsMessageRepository repeatedViaSecondsMessageRepository, IUnitOfWork unitOfWork)
    {
        _scheduledMessageRepository = scheduledMessageRepository;
        _repeatedViaMinutesMessageRepository = repeatedViaMinutesMessageRepository;
        _repeatedViaSecondsMessageRepository = repeatedViaSecondsMessageRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Add(Guid id, Timer timer)
    {
        _scheduledMessageTimers.Add(id, timer);

        return Task.CompletedTask;
    }

    public Task Add(Guid id, RepeatedMessageTimers timers)
    {
        _repeatedMessageTimers.Add(id, timers);

        return Task.CompletedTask;
    }

    public async Task RemoveScheduledMessageTimer(Guid id, CancellationToken cancellationToken)
    {
        if (!_scheduledMessageTimers.ContainsKey(id))
        {
            throw new TimerNotExistsException($"Timer for message with id: {id} doesn't exist!");
        }

        _scheduledMessageTimers[id].Close();

        await _scheduledMessageRepository.Remove(id, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _scheduledMessageTimers.Remove(id);
    }

    public async Task RemoveRepeatedViaMinutesMessageTimers(Guid id, CancellationToken cancellationToken)
    {
        if (!_repeatedMessageTimers.ContainsKey(id))
        {
            throw new TimerNotExistsException($"Timers for message with id: {id} don't exist!");
        }

        await _repeatedViaMinutesMessageRepository.Remove(id, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        DeleteRepeatedMessageTimer(id);
    }

    public async Task RemoveRepeatedViaSecondsMessageTimers(Guid id, CancellationToken cancellationToken)
    {
        if (!_repeatedMessageTimers.ContainsKey(id))
        {
            throw new TimerNotExistsException($"Timers for message with id: {id} don't exist!");
        }

        await _repeatedViaSecondsMessageRepository.Remove(id, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        DeleteRepeatedMessageTimer(id);
    }

    private void DeleteRepeatedMessageTimer(Guid id)
    {
        _repeatedMessageTimers[id].RepeatedTimer.Close();
        _repeatedMessageTimers[id].EndTimer.Close();

        _repeatedMessageTimers.Remove(id);
    }
}