using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;
using Message = NothingToForgetBot.Core.Messages.Models.Message;
using Timer = System.Timers.Timer;

namespace NothingToForgetBot.Core.Messages.Handlers.Implementations;

public class ScheduledMessageHandler : IScheduledMessageHandler
{
    private readonly IScheduledMessageRepository _scheduledMessageRepository;

    private readonly IRepeatedViaMinutesScheduledMessageRepository _repeatedViaMinutesScheduledMessageRepository;

    private readonly IRepeatedViaSecondsScheduledMessageRepository _repeatedViaSecondsScheduledMessageRepository;

    private readonly IMessageSender _messageSender;


    public ScheduledMessageHandler(IScheduledMessageRepository scheduledMessageRepository,
        IRepeatedViaMinutesScheduledMessageRepository repeatedViaMinutesScheduledMessageRepository,
        IRepeatedViaSecondsScheduledMessageRepository repeatedViaSecondsScheduledMessageRepository,
        IMessageSender messageSender)
    {
        _scheduledMessageRepository = scheduledMessageRepository;
        _repeatedViaMinutesScheduledMessageRepository = repeatedViaMinutesScheduledMessageRepository;
        _repeatedViaSecondsScheduledMessageRepository = repeatedViaSecondsScheduledMessageRepository;
        _messageSender = messageSender;
    }

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        switch (message)
        {
            case ScheduledMessage scheduledMessage:
                await HandleScheduledMessage(scheduledMessage, cancellationToken);
                break;
            case RepeatedViaMinutesScheduledMessage repeatedViaMinutesScheduledMessage:
                await HandleRepeatedViaMinutesScheduledMessage(repeatedViaMinutesScheduledMessage, cancellationToken);
                break;
            case RepeatedViaSecondsScheduledMessage repeatedViaSecondsScheduledMessage:
                await HandleRepeatedViaSecondsScheduledMessage(repeatedViaSecondsScheduledMessage, cancellationToken);
                break;
        }
    }

    private async Task HandleScheduledMessage(ScheduledMessage scheduledMessage, CancellationToken cancellationToken)
    {
        await _scheduledMessageRepository.Add(scheduledMessage, cancellationToken);

        var interval = (scheduledMessage.PublishingDate - DateTime.Now).TotalMilliseconds;

        var timer = new Timer(interval);

        timer.Elapsed += async (_, _) =>
        {
            await _messageSender.SendMessageToChat(scheduledMessage.ChatId, scheduledMessage.Content, cancellationToken);

            await _scheduledMessageRepository.Remove(scheduledMessage.Id, cancellationToken);

            timer.Close();
        };

        timer.AutoReset = false;
        timer.Enabled = true;
    }

    private async Task HandleRepeatedViaMinutesScheduledMessage(RepeatedViaMinutesScheduledMessage scheduledMessage,
        CancellationToken cancellationToken)
    {
        await _repeatedViaMinutesScheduledMessageRepository.Add(scheduledMessage, cancellationToken);

        var repeatedTimerInterval = scheduledMessage.Interval * 60000;

        var endTimerInterval = CalculateEndTimerInterval(scheduledMessage);
        
        await HandleRepeatedMessage(scheduledMessage, repeatedTimerInterval, endTimerInterval, cancellationToken);
    }

    private async Task HandleRepeatedViaSecondsScheduledMessage(RepeatedViaSecondsScheduledMessage scheduledMessage,
        CancellationToken cancellationToken)
    {
        await _repeatedViaSecondsScheduledMessageRepository.Add(scheduledMessage, cancellationToken);

        var repeatedTimerInterval = scheduledMessage.Interval * 1000;

        var endTimerInterval = CalculateEndTimerInterval(scheduledMessage);

        await HandleRepeatedMessage(scheduledMessage, repeatedTimerInterval, endTimerInterval, cancellationToken);
    }

    private Task HandleRepeatedMessage(Message message, int repeatedTimerInterval, double endTimerInterval,
        CancellationToken cancellationToken)
    {
        var repeatedTimer = new Timer(repeatedTimerInterval);

        repeatedTimer.Elapsed += async (_, _) =>
        {
            await _messageSender.SendMessageToChat(message.ChatId, message.Content, cancellationToken);
        };

        repeatedTimer.AutoReset = true;

        var endTimer = new Timer(endTimerInterval);

        endTimer.Elapsed += async (_, _) =>
        {
            await _messageSender.SendMessageToChat(message.ChatId, message.Content, cancellationToken);

            switch (message)
            {
                case RepeatedViaMinutesScheduledMessage repeatedViaMinutesScheduledMessage:
                    await _repeatedViaMinutesScheduledMessageRepository.Remove(repeatedViaMinutesScheduledMessage.Id,
                        cancellationToken);
                    break;
                case RepeatedViaSecondsScheduledMessage repeatedViaSecondsScheduledMessage:
                    await _repeatedViaSecondsScheduledMessageRepository.Remove(repeatedViaSecondsScheduledMessage.Id,
                        cancellationToken);
                    break;
            }

            repeatedTimer.Close();

            endTimer.Close();
        };

        endTimer.AutoReset = false;

        repeatedTimer.Enabled = true;
        endTimer.Enabled = true;
        
        return Task.CompletedTask;
    }

    private double CalculateEndTimerInterval(RepeatedMessage message)
    {
        return (message.EndDate - DateTime.Now).TotalMilliseconds;
    }
}