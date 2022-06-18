namespace NothingToForgetBot.Core.Messages.Models;

public class RepeatedViaSecondsScheduledMessage : Message
{
    public int Interval { get; set; }

    public DateTime EndDate { get; set; }
}