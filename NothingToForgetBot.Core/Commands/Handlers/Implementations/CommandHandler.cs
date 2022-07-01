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

    public async Task Handle(Command command, long chatId, CancellationToken cancellationToken)
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

    private async Task RespondOnListCommand(long chatId, CancellationToken cancellationToken)
    {
        var userRecords = await _userRecordSelector.Select(chatId, cancellationToken);

        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatId(chatId, cancellationToken);

        var message = string.Empty;

        var counter = 0;

        foreach (var scheduledMessage in userRecords.ScheduledMessages)
        {
            message += $"{counter}) {scheduledMessage.ToString()}\n";
            counter++;
        }

        var every = _resourceReader.GetString(currentLanguage + "Every");
        var until = _resourceReader.GetString(currentLanguage + "Until");
        var minutes = _resourceReader.GetString(currentLanguage + "Minutes");
        var seconds = _resourceReader.GetString(currentLanguage + "Seconds");

        foreach (var scheduledMessage in userRecords.RepeatedViaMinutesScheduledMessages)
        {
            message += $"{counter}) {scheduledMessage.ToString(every, minutes, until)}\n";
            counter++;
        }

        foreach (var scheduledMessage in userRecords.RepeatedViaSecondsScheduledMessages)
        {
            message += $"{counter}) {scheduledMessage.ToString(every, seconds, until)}\n";
            counter++;
        }

        foreach (var note in userRecords.Notes)
        {
            message += $"{counter}) {note.ToString()}\n";
            counter++;
        }

        await _messageSender.SendMessageToChat(chatId, message, cancellationToken);
    }
}