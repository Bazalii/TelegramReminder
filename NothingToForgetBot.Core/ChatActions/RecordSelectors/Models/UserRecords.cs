using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.ChatActions.RecordSelectors.Models;

public class UserRecords
{
    public List<ScheduledMessage> ScheduledMessages { get; set; } = new();

    public List<RepeatedViaMinutesMessage> RepeatedViaMinutesMessages { get; set; } = new();

    public List<RepeatedViaSecondsMessage> RepeatedViaSecondsMessages { get; set; } = new();

    public List<Note> Notes { get; set; } = new();
}