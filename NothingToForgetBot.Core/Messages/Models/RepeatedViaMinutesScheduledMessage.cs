namespace NothingToForgetBot.Core.Messages.Models;

public class RepeatedViaMinutesScheduledMessage : Message
{
    public int Interval { get; set; }

    public DateTime EndDate { get; set; }
}