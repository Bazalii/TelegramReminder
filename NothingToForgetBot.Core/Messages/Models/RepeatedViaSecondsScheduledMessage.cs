using System.Globalization;

namespace NothingToForgetBot.Core.Messages.Models;

public class RepeatedViaSecondsScheduledMessage : RepeatedMessage
{
    public string ToString(string every, string seconds, string until)
    {
        return
            $"{Content} {every} {Interval} {seconds} {until} {EndDate.ToString(CultureInfo.GetCultureInfo("ru-RU"))}";
    }
}