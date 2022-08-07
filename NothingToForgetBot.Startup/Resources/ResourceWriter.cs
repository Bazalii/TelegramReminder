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
        resx.AddResource("EnGuide",
            "This bot can be used for scheduling notifications or handling your notes.\n" +
            "You can schedule a notification that will be published once: eat in 2 min, go to bed in 30 sec.\n" +
            "It is also possible to set the concrete publishing date: eat at 18:30.\n" +
            "Furthermore you can set repeated notifications that will be published until specific time:\n" +
            "jump every 5 sec until 12:30, play with dog every 30 min until 15:17.\n\n" +
            "There is no pattern for notes, so just write down your thoughts and they will be saved.\n\n" +
            "It's worth mentioning that you can delete a notification or a note.\n" +
            "For that use /delete command using section and notification or note number inside this section:" +
            "/delete IV 1, /delete I 10.\n\n" +
            "Other available commands you can get in menu inside chat with this bot");

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
        resx.AddResource("RuGuide", 
            "Этот бот может быть использован для отложенных уведомлений и заметок.\n" +
            "Можно отложить однократное уведомление: поесть через 2 мин, пойти спать через 30 сек.\n" +
            "Ещё возможно задать конкретную дату уведомления: покушать в 18:30.\n" +
            "Более того, поддерживаются уведомления, которые будут повторяться до определённого времени:\n" +
            "прыгать каждые 5 сек до 12:30, играть с собакой каждые 30 мин до 15:17.\n\n" +
            "Для заметок отсутствует определённая форма сообщений, поэтому пишите любые мысли и они будут сохранены.\n\n" +
            "Стоит отметить, существует возможность удаления напоминаний и заметок.\n" +
            "Для этого используйте команду /delete, указывая секцию и номер сообщения или заметки внутри секции:" +
            "/delete IV 1, /delete I 10.\n\n" +
            "Чтобы посмотреть список доступных команд воспользуйтесь меню внутри чата с данным ботом");

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