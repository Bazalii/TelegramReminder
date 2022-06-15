using System.Resources.NetStandard;

namespace NothingToForgetBot.Core.Resources;

public class ResourceWriter
{
    public static void Write()
    {
        using var resx = new ResXResourceWriter("ApplicationResources.resx");

        //Commands 
        resx.AddResource("CommandGuide", "/guide");
        resx.AddResource("CommandLanguage", "/language");
        resx.AddResource("CommandList", "/list");

        //Separators
        resx.AddResource("TimeSeparator", ":");
        resx.AddResource("DateSeparator", ".");


        //English localisation

        //Guide
        resx.AddResource("EnGuide", "BlaBla");

        //Scheduling markers
        resx.AddResource("EnAt", "at");
        resx.AddResource("EnIn", "in");
        resx.AddResource("EnEvery", "every");
        resx.AddResource("EnUntil", "until");
        resx.AddResource("EnMinutes", "min");
        resx.AddResource("EnSeconds", "sec");

        //Exception messages
        resx.AddResource("EnPublishingDateIsEarlierThanNowExceptionMessage",
            "Publishing date cannot be earlier than now!");
        resx.AddResource("EnEndDateIsEarlierThanNowExceptionMessage",
            "End date of notifications publishing cannot be earlier than now!");


        //Russian localisation

        //Guide
        resx.AddResource("RuGuide", "БлаБла");

        //Scheduling markers
        resx.AddResource("RuAt", "в");
        resx.AddResource("RuIn", "через");
        resx.AddResource("RuEvery", "каждые");
        resx.AddResource("RuUntil", "до");
        resx.AddResource("RuMinutes", "мин");
        resx.AddResource("RuSeconds", "сек");

        //Exception messages
        resx.AddResource("RuPublishingDateIsEarlierThanNowExceptionMessage",
            "Дата публикации не может быть раньше текущего момента!");
        resx.AddResource("RuEndDateIsEarlierThanNowExceptionMessage",
            "Дата окончания публикации уведомлений не может быть раньше текущего момента!");
    }
}