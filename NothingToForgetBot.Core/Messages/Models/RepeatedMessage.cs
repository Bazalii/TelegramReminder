namespace NothingToForgetBot.Core.Messages.Models;

public abstract class RepeatedMessage : Message
{
    public int Interval { get; set; }
    public DateTime EndDate { get; set; }
}