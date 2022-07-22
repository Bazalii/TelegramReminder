using NothingToForgetBot.Core.ChatActions.RecordSelectors.Models;
using NothingToForgetBot.Core.Messages.Selectors;
using NothingToForgetBot.Core.Notes.Selectors;

namespace NothingToForgetBot.Core.ChatActions.RecordSelectors.Implementations;

public class UserRecordSelector : IUserRecordSelector
{
    private readonly IUserMessageSelector _messageSelector;

    private readonly IUserNoteSelector _noteSelector;


    public UserRecordSelector(IUserMessageSelector messageSelector, IUserNoteSelector noteSelector)
    {
        _messageSelector = messageSelector;
        _noteSelector = noteSelector;
    }

    public async Task<UserRecords> Select(long chatId, CancellationToken cancellationToken)
    {
        var userMessages = await _messageSelector.Select(chatId, cancellationToken);

        var userNotes = await _noteSelector.Select(chatId, cancellationToken);

        return new UserRecords
        {
            ScheduledMessages = userMessages.ScheduledMessages,
            RepeatedViaMinutesMessages = userMessages.RepeatedViaMinutesMessages,
            RepeatedViaSecondsScheduledMessages = userMessages.RepeatedViaSecondsScheduledMessages,
            Notes = userNotes
        };
    }
}