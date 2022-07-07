using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.ChatActions.RecordSelectors.Models;

public class UserRecords
{
    public List<ScheduledMessage> ScheduledMessages { get; set; }

    public List<RepeatedViaMinutesScheduledMessage> RepeatedViaMinutesScheduledMessages { get; set; }

    public List<RepeatedViaSecondsScheduledMessage> RepeatedViaSecondsScheduledMessages { get; set; }

    public List<Note> Notes { get; set; }
}