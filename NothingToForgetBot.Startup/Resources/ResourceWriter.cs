using System.Resources.NetStandard;

namespace NothingToForgetBot.Startup.Resources;

public class ResourceWriter
{
    public static void Write()
    {
        using var resx = new ResXResourceWriter("ApplicationResources.resx");

        //Commands 
        resx.AddResource("CommandGuide", "/guide");
        resx.AddResource("CommandLanguage", "/language");
        resx.AddResource("CommandList", "/list");
        resx.AddResource("CommandDelete", "/delete");
        resx.AddResource("CommandSetTimeZone", "/setTimeZone");

        //Separators
        resx.AddResource("TimeSeparator", ":");
        resx.AddResource("DateSeparator", ".");

        //Sections
        resx.AddResource("ScheduledMessagesSection", "I");
        resx.AddResource("RepeatedViaMinutesMessagesSection", "II");
        resx.AddResource("RepeatedViaSecondsMessagesSection", "III");
        resx.AddResource("NotesSection", "IV");
        
        //Defaults
        resx.AddResource("DefaultLanguage", "En");

        //English localisation

        //Guide
        resx.AddResource("EnGuide", "BlaBla");

        //Language
        resx.AddResource("EnLanguage", "BlaBlaBla");

        //Scheduling markers
        resx.AddResource("EnAt", "at");
        resx.AddResource("EnIn", "in");
        resx.AddResource("EnEvery", "every");
        resx.AddResource("EnUntil", "until");
        resx.AddResource("EnMinutes", "min");
        resx.AddResource("EnSeconds", "sec");

        //Messages
        resx.AddResource("EnPublishingDateIsEarlierThanNowExceptionMessage",
            "Publishing date cannot be earlier than now!");
        resx.AddResource("EnEndDateIsEarlierThanNowExceptionMessage",
            "End date of notifications publishing cannot be earlier than now!");
        resx.AddResource("EnObjectNumberIsBiggerThanAmountInSectionMessage",
            "The number of the object that you wanted to delete is bigger than total amount of objects in the section!");

        //Object names
        resx.AddResource("EnScheduledMessages", "Scheduled notifications");
        resx.AddResource("EnRepeatedViaMinutesMessages", "Repeated via minutes notifications");
        resx.AddResource("EnRepeatedViaSecondsMessages", "Repeated via seconds notifications");
        resx.AddResource("EnNotes", "Notes");

        //Russian localisation

        //Guide
        resx.AddResource("RuGuide", "БлаБла");

        //Language
        resx.AddResource("RuLanguage", "БлаБлаБла");

        //Scheduling markers
        resx.AddResource("RuAt", "в");
        resx.AddResource("RuIn", "через");
        resx.AddResource("RuEvery", "каждые");
        resx.AddResource("RuUntil", "до");
        resx.AddResource("RuMinutes", "мин");
        resx.AddResource("RuSeconds", "сек");

        //Messages
        resx.AddResource("RuPublishingDateIsEarlierThanNowExceptionMessage",
            "Дата публикации не может быть раньше текущего момента!");
        resx.AddResource("RuEndDateIsEarlierThanNowExceptionMessage",
            "Дата окончания публикации уведомлений не может быть раньше текущего момента!");
        resx.AddResource("RuObjectNumberIsBiggerThanAmountInSectionMessage",
            "Вы ввели номер объекта, который больше, чем количество объектов в секции!");

        //Object names
        resx.AddResource("RuScheduledMessages", "Отложенные уведомления");
        resx.AddResource("RuRepeatedViaMinutesMessages", "Повторяемые через секунды уведомления");
        resx.AddResource("RuRepeatedViaSecondsMessages", "Повторяемые через минуты уведомления");
        resx.AddResource("RuNotes", "Заметки");
    }
}