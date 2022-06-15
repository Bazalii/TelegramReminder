namespace NothingToForgetBot.Core.Messages;

public class RepeatedViaMinutesScheduledMessage : Message
{
    public int Interval { get; set; }

    public DateTime EndDate { get; set; }
}