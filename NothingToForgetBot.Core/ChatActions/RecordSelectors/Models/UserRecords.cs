using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.ChatActions.RecordSelectors.Models;

public class UserRecords
{
    public List<ScheduledMessage> ScheduledMessages { get; init; } = new();

    public List<RepeatedViaMinutesMessage> RepeatedViaMinutesMessages { get; init; } = new();

    public List<RepeatedViaSecondsMessage> RepeatedViaSecondsMessages { get; init; } = new();

    public List<Note> Notes { get; init; } = new();
}