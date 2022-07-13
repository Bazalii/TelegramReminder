namespace NothingToForgetBot.Data.Messages.Models;

public class RepeatedViaMinutesMessageDbModel : MessageDbModel
{
    public int Interval { get; set; }

    public DateTime EndDate { get; set; }
}