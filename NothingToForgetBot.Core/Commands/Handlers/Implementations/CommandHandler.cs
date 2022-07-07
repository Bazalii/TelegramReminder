using System.Resources.NetStandard;
using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.ChatActions.RecordSelectors;
using NothingToForgetBot.Core.Enums;
using NothingToForgetBot.Core.Languages.Repository;

namespace NothingToForgetBot.Core.Commands.Handlers.Implementations;

public class CommandHandler : ICommandHandler
{
    private readonly IMessageSender _messageSender;

    private readonly ResXResourceReader _resourceReader;

    private readonly IChatLanguageRepository _chatLanguageRepository;

    private readonly IUserRecordSelector _userRecordSelector;

    public CommandHandler(IMessageSender messageSender, ResXResourceReader resourceReader,
        IChatLanguageRepository chatLanguageRepository, IUserRecordSelector userRecordSelector)
    {
        _messageSender = messageSender;
        _resourceReader = resourceReader;
        _chatLanguageRepository = chatLanguageRepository;
        _userRecordSelector = userRecordSelector;
    }

    public async Task Handle(Command command, long chatId, string localisation, CancellationToken cancellationToken)
    {
        switch (command)
        {
            case Command.Guide:
                await RespondOnGuideCommand(chatId, cancellationToken);
                break;
            case Command.Language:
                await RespondOnLanguageCommand(chatId, cancellationToken);
                break;
            case Command.List:
                await RespondOnListCommand(chatId, localisation, cancellationToken);
                break;
        }
    }

    private async Task RespondOnGuideCommand(long chatId, CancellationToken cancellationToken)
    {
        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatId(chatId, cancellationToken);

        var message = _resourceReader.GetString($"{currentLanguage}Language");

        await _messageSender.SendMessageToChat(chatId, message, cancellationToken);
    }

    private async Task RespondOnLanguageCommand(long chatId, CancellationToken cancellationToken)
    {
        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatId(chatId, cancellationToken);

        var message = _resourceReader.GetString($"{currentLanguage}Language");

        await _messageSender.SendMessageToChat(chatId, message, cancellationToken);
    }

    private async Task RespondOnListCommand(long chatId, string localisation, CancellationToken cancellationToken)
    {
        var userRecords = await _userRecordSelector.Select(chatId, cancellationToken);

        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatId(chatId, cancellationToken);

        var message = string.Empty;

        var scheduledMessagesSectionResourceValue = _resourceReader.GetString("ScheduledMessagesSection");
        var scheduledMessagesResourceValue = _resourceReader.GetString(localisation + "ScheduledMessages");

        message += $"{scheduledMessagesSectionResourceValue}. {scheduledMessagesResourceValue}\n";

        for (var i = 0; i < userRecords.ScheduledMessages.Count; i++)
        {
            message += $"{i + 1}) {userRecords.ScheduledMessages[i].ToString()}\n";
        }

        var every = _resourceReader.GetString(currentLanguage + "Every");
        var until = _resourceReader.GetString(currentLanguage + "Until");
        var minutes = _resourceReader.GetString(currentLanguage + "Minutes");
        var seconds = _resourceReader.GetString(currentLanguage + "Seconds");

        var repeatedViaMinutesMessagesSectionResourceValue =
            _resourceReader.GetString("RepeatedViaMinutesMessagesSection");
        var repeatedViaMinutesMessagesResourceValue =
            _resourceReader.GetString(localisation + "RepeatedViaMinutesMessages");

        message += $"{repeatedViaMinutesMessagesSectionResourceValue}. {repeatedViaMinutesMessagesResourceValue}\n";

        for (var i = 0; i < userRecords.RepeatedViaMinutesScheduledMessages.Count; i++)
        {
            message +=
                $"{i + 1}) {userRecords.RepeatedViaMinutesScheduledMessages[i].ToString(every, minutes, until)}\n";
        }

        var repeatedViaSecondsMessagesSectionResourceValue =
            _resourceReader.GetString("RepeatedViaSecondsMessagesSection");
        var repeatedViaSecondsMessagesResourceValue =
            _resourceReader.GetString(localisation + "RepeatedViaSecondsMessages");

        message += $"{repeatedViaSecondsMessagesSectionResourceValue}. {repeatedViaSecondsMessagesResourceValue}\n";

        for (var i = 0; i < userRecords.RepeatedViaSecondsScheduledMessages.Count; i++)
        {
            message +=
                $"{i + 1}) {userRecords.RepeatedViaSecondsScheduledMessages[i].ToString(every, seconds, until)}\n";
        }

        var notesSectionResourceValue = _resourceReader.GetString("NotesSection");
        var notesResourceValue = _resourceReader.GetString(localisation + "Notes");

        message += $"{notesSectionResourceValue}. {notesResourceValue}\n";

        for (var i = 0; i < userRecords.Notes.Count; i++)
        {
            message += $"{i + 1}) {userRecords.Notes[i].ToString()}\n";
        }

        await _messageSender.SendMessageToChat(chatId, message, cancellationToken);
    }
}