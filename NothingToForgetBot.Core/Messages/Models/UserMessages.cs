namespace NothingToForgetBot.Core.Messages.Models;

public class UserMessages
{
    public List<ScheduledMessage> ScheduledMessages { get; set; } = new();

    public List<RepeatedViaMinutesMessage> RepeatedViaMinutesMessages { get; set; } = new();

    public List<RepeatedViaSecondsMessage> RepeatedViaSecondsMessages { get; set; } = new();
}