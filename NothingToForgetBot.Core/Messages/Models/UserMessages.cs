namespace NothingToForgetBot.Core.Messages.Models;

public class UserMessages
{
    public List<ScheduledMessage> ScheduledMessages { get; init; } = new();
    public List<RepeatedViaMinutesMessage> RepeatedViaMinutesMessages { get; init; } = new();
    public List<RepeatedViaSecondsMessage> RepeatedViaSecondsMessages { get; init; } = new();
}