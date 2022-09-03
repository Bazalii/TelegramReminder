using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Core.Timers.Handlers;
using NothingToForgetBot.Core.Timers.Models;
using Message = NothingToForgetBot.Core.Messages.Models.Message;
using Timer = System.Timers.Timer;

namespace NothingToForgetBot.Core.Messages.Handlers.Implementations;

public class ScheduledMessageHandler : IScheduledMessageHandler
{
    private readonly IScheduledMessageRepository _scheduledMessageRepository;
    private readonly IRepeatedViaMinutesMessageRepository _repeatedViaMinutesMessageRepository;
    private readonly IRepeatedViaSecondsMessageRepository _repeatedViaSecondsMessageRepository;
    private readonly IMessageSender _messageSender;
    private readonly ITimerHandler _timerHandler;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduledMessageHandler(IScheduledMessageRepository scheduledMessageRepository,
        IRepeatedViaMinutesMessageRepository repeatedViaMinutesMessageRepository,
        IRepeatedViaSecondsMessageRepository repeatedViaSecondsMessageRepository,
        IMessageSender messageSender, ITimerHandler timerHandler, IUnitOfWork unitOfWork)
    {
        _scheduledMessageRepository = scheduledMessageRepository;
        _repeatedViaMinutesMessageRepository = repeatedViaMinutesMessageRepository;
        _repeatedViaSecondsMessageRepository = repeatedViaSecondsMessageRepository;
        _messageSender = messageSender;
        _timerHandler = timerHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        switch (message)
        {
            case ScheduledMessage scheduledMessage:
                await HandleScheduledMessage(scheduledMessage, cancellationToken);
                break;
            case RepeatedViaMinutesMessage repeatedViaMinutesMessage:
                await HandleRepeatedViaMinutesMessage(repeatedViaMinutesMessage, cancellationToken);
                break;
            case RepeatedViaSecondsMessage repeatedViaSecondsMessage:
                await HandleRepeatedViaSecondsMessage(repeatedViaSecondsMessage, cancellationToken);
                break;
        }
    }

    private async Task HandleScheduledMessage(ScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        await _scheduledMessageRepository.Add(scheduledMessage, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        var interval = (scheduledMessage.PublishingDate - DateTime.UtcNow).TotalMilliseconds;

        var timer = new Timer(interval);

        timer.Elapsed += async (_, _) =>
        {
            await _messageSender.SendMessageToChat(scheduledMessage.ChatId, scheduledMessage.Content,
                cancellationToken);

            await _scheduledMessageRepository.Remove(scheduledMessage.Id, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            timer.Close();
        };

        timer.AutoReset = false;
        timer.Enabled = true;

        await _timerHandler.Add(scheduledMessage.Id, timer);
    }

    private async Task HandleRepeatedViaMinutesMessage(RepeatedViaMinutesMessage repeatedMessage,
        CancellationToken cancellationToken)
    {
        await _repeatedViaMinutesMessageRepository.Add(repeatedMessage, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        var repeatedTimerInterval = repeatedMessage.Interval * 60000;

        var endTimerInterval = CalculateEndTimerInterval(repeatedMessage);

        await HandleRepeatedMessage(repeatedMessage, repeatedTimerInterval, endTimerInterval, cancellationToken);
    }

    private async Task HandleRepeatedViaSecondsMessage(RepeatedViaSecondsMessage repeatedMessage,
        CancellationToken cancellationToken)
    {
        await _repeatedViaSecondsMessageRepository.Add(repeatedMessage, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        var repeatedTimerInterval = repeatedMessage.Interval * 1000;

        var endTimerInterval = CalculateEndTimerInterval(repeatedMessage);

        await HandleRepeatedMessage(repeatedMessage, repeatedTimerInterval, endTimerInterval, cancellationToken);
    }

    private async Task HandleRepeatedMessage(RepeatedMessage repeatedMessage, int repeatedTimerInterval,
        double endTimerInterval,
        CancellationToken cancellationToken)
    {
        var repeatedTimer = new Timer(repeatedTimerInterval);

        repeatedTimer.Elapsed += async (_, _) =>
        {
            await _messageSender.SendMessageToChat(repeatedMessage.ChatId, repeatedMessage.Content,
                cancellationToken);
        };

        repeatedTimer.AutoReset = true;

        var endTimer = new Timer(endTimerInterval);

        endTimer.Elapsed += async (_, _) =>
        {
            await _messageSender.SendMessageToChat(repeatedMessage.ChatId, repeatedMessage.Content, cancellationToken);

            switch (repeatedMessage)
            {
                case RepeatedViaMinutesMessage repeatedViaMinutesMessage:
                    await _repeatedViaMinutesMessageRepository.Remove(repeatedViaMinutesMessage.Id, cancellationToken);
                    break;
                case RepeatedViaSecondsMessage repeatedViaSecondsMessage:
                    await _repeatedViaSecondsMessageRepository.Remove(repeatedViaSecondsMessage.Id, cancellationToken);
                    break;
            }

            await _unitOfWork.SaveChanges(cancellationToken);

            repeatedTimer.Close();

            endTimer.Close();
        };

        endTimer.AutoReset = false;

        repeatedTimer.Enabled = true;
        endTimer.Enabled = true;

        var repeatedMessageTimers = new RepeatedMessageTimers
        {
            RepeatedTimer = repeatedTimer,
            EndTimer = endTimer
        };

        await _timerHandler.Add(repeatedMessage.Id, repeatedMessageTimers);
    }

    private static double CalculateEndTimerInterval(RepeatedMessage message)
    {
        return (message.EndDate - DateTime.UtcNow).TotalMilliseconds;
    }
}