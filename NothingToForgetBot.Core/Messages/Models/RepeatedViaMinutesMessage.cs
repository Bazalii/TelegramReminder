using System.Globalization;

namespace NothingToForgetBot.Core.Messages.Models;

public class RepeatedViaMinutesMessage : RepeatedMessage
{
    public string ToString(string every, string minutes, string until)
    {
        return
            $"{Content} {every} {Interval} {minutes} {until} {EndDate.ToString(CultureInfo.GetCultureInfo("ru-RU"))}";
    }
}