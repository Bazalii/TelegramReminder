namespace NothingToForgetBot.Data.Messages.Models;

public class RepeatedViaSecondsMessageDbModel : MessageDbModel
{
    public int Interval { get; set; }

    public DateTime EndDate { get; set; }
}