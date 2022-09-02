using System.Resources.NetStandard;
using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.ChatActions.RecordSelectors;
using NothingToForgetBot.Core.Enums;
using NothingToForgetBot.Core.Languages;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Core.TimeZones.Repositories;

namespace NothingToForgetBot.Core.Commands.Handlers.Implementations;

public class CommandHandler : ICommandHandler
{
    private readonly IMessageSender _messageSender;
    private readonly ResXResourceReader _resourceReader;
    private readonly IChatLanguageRepository _chatLanguageRepository;
    private readonly ITimeZoneRepository _timeZoneRepository;
    private readonly IUserRecordSelector _userRecordSelector;
    private readonly IUnitOfWork _unitOfWork;

    public CommandHandler(IMessageSender messageSender, ResXResourceReader resourceReader,
        IChatLanguageRepository chatLanguageRepository, ITimeZoneRepository timeZoneRepository,
        IUserRecordSelector userRecordSelector, IUnitOfWork unitOfWork)
    {
        _messageSender = messageSender;
        _resourceReader = resourceReader;
        _chatLanguageRepository = chatLanguageRepository;
        _timeZoneRepository = timeZoneRepository;
        _userRecordSelector = userRecordSelector;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(Command command, long chatId, string message, CancellationToken cancellationToken)
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
                await RespondOnListCommand(chatId, cancellationToken);
                break;
            case Command.SetLanguage:
                await RespondOnSetLanguageCommand(chatId, message, cancellationToken);
                break;
        }
    }

    private async Task RespondOnGuideCommand(long chatId, CancellationToken cancellationToken)
    {
        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatIdOrDefault(chatId, cancellationToken);

        var message = _resourceReader.GetString($"{currentLanguage}Guide");

        await _messageSender.SendMessageToChat(chatId, message, cancellationToken);
    }

    private async Task RespondOnLanguageCommand(long chatId, CancellationToken cancellationToken)
    {
        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatIdOrDefault(chatId, cancellationToken);

        var message = _resourceReader.GetString($"{currentLanguage}Language");

        await _messageSender.SendMessageToChat(chatId, message, cancellationToken);
    }

    private async Task RespondOnSetLanguageCommand(long chatId, string message, CancellationToken cancellationToken)
    {
        var chatWithLanguage = new ChatWithLanguage
        {
            ChatId = chatId,
            Language = message[1].ToString().ToUpper() + message[2]
        };

        var chatLanguage = await _chatLanguageRepository.GetLanguageByChatId(chatId, cancellationToken);

        if (chatLanguage is null)
        {
            await _chatLanguageRepository.Add(chatWithLanguage, cancellationToken);
        }
        else
        {
            await _chatLanguageRepository.Update(chatWithLanguage, cancellationToken);
        }

        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task RespondOnListCommand(long chatId, CancellationToken cancellationToken)
    {
        var localisation = await _chatLanguageRepository.GetLanguageByChatIdOrDefault(chatId, cancellationToken);

        var userRecords = await _userRecordSelector.Select(chatId, cancellationToken);

        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatIdOrDefault(chatId, cancellationToken);

        var chatTimeZone = await _timeZoneRepository.GetByChatId(chatId, cancellationToken);
        var timeZone = chatTimeZone.TimeZone;

        var message = string.Empty;

        var scheduledMessagesSectionResourceValue = _resourceReader.GetString("ScheduledMessagesSection");
        var scheduledMessagesResourceValue = _resourceReader.GetString(localisation + "ScheduledMessages");

        message += $"{scheduledMessagesSectionResourceValue}. {scheduledMessagesResourceValue}\n";

        for (var i = 0; i < userRecords.ScheduledMessages.Count; i++)
        {
            userRecords.ScheduledMessages[i].PublishingDate += TimeSpan.FromHours(timeZone);
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

        for (var i = 0; i < userRecords.RepeatedViaMinutesMessages.Count; i++)
        {
            userRecords.RepeatedViaMinutesMessages[i].EndDate += TimeSpan.FromHours(timeZone);
            message +=
                $"{i + 1}) {userRecords.RepeatedViaMinutesMessages[i].ToString(every, minutes, until)}\n";
        }

        var repeatedViaSecondsMessagesSectionResourceValue =
            _resourceReader.GetString("RepeatedViaSecondsMessagesSection");
        var repeatedViaSecondsMessagesResourceValue =
            _resourceReader.GetString(localisation + "RepeatedViaSecondsMessages");

        message += $"{repeatedViaSecondsMessagesSectionResourceValue}. {repeatedViaSecondsMessagesResourceValue}\n";

        for (var i = 0; i < userRecords.RepeatedViaSecondsMessages.Count; i++)
        {
            userRecords.RepeatedViaSecondsMessages[i].EndDate += TimeSpan.FromHours(timeZone);
            message +=
                $"{i + 1}) {userRecords.RepeatedViaSecondsMessages[i].ToString(every, seconds, until)}\n";
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