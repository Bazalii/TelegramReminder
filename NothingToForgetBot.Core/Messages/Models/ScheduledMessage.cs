using System.Globalization;

namespace NothingToForgetBot.Core.Messages.Models;

public class ScheduledMessage : Message
{
    public DateTime PublishingDate { get; set; }

    public override string ToString()
    {
        return $"{Content} {PublishingDate.ToString(CultureInfo.GetCultureInfo("ru-RU"))}";
    }
}