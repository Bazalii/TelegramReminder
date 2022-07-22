using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.ChatActions.RecordSelectors;
using NothingToForgetBot.Core.Enums;
using NothingToForgetBot.Core.Notes.Repositories;
using NothingToForgetBot.Core.Timers.Handlers;

namespace NothingToForgetBot.Core.Commands.Handlers.Implementations;

public class CommandWithArgumentsHandler : ICommandWithArgumentsHandler
{
    private readonly ITimerHandler _timerHandler;

    private readonly INoteRepository _noteRepository;

    private readonly ResXResourceReader _resourceReader;

    private readonly IUserRecordSelector _userRecordSelector;

    private readonly IMessageSender _messageSender;

    private readonly IUnitOfWork _unitOfWork;

    public CommandWithArgumentsHandler(ITimerHandler timerHandler, INoteRepository noteRepository,
        ResXResourceReader resourceReader, IUserRecordSelector userRecordSelector, IMessageSender messageSender,
        IUnitOfWork unitOfWork)
    {
        _timerHandler = timerHandler;
        _noteRepository = noteRepository;
        _resourceReader = resourceReader;
        _userRecordSelector = userRecordSelector;
        _messageSender = messageSender;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CommandWithArguments command, long chatId, string localisation, string message,
        CancellationToken cancellationToken)
    {
        switch (command)
        {
            case CommandWithArguments.Delete:
                await HandleDeleteCommand(chatId, message, localisation, cancellationToken);
                break;
        }
    }

    public async Task HandleDeleteCommand(long chatId, string message, string localisation,
        CancellationToken cancellationToken)
    {
        var userRecords = await _userRecordSelector.Select(chatId, cancellationToken);

        var deleteCommand = _resourceReader.GetString("CommandDelete");
        var scheduledMessagesSection = _resourceReader.GetString("ScheduledMessagesSection");
        var repeatedViaMinutesMessagesSection = _resourceReader.GetString("RepeatedViaMinutesMessagesSection");
        var repeatedViaSecondsMessagesSection = _resourceReader.GetString("RepeatedViaSecondsMessagesSection");
        var notesSection = _resourceReader.GetString("NotesSection");

        var scheduledMessageDeletionRegex = new Regex($"{deleteCommand}\\s{scheduledMessagesSection}\\s\\d");
        var repeatedViaMinutesMessageDeletionRegex =
            new Regex($"{deleteCommand}\\s{repeatedViaMinutesMessagesSection}\\s\\d");
        var repeatedViaSecondsMessageDeletionRegex =
            new Regex($"{deleteCommand}\\s{repeatedViaSecondsMessagesSection}\\s\\d");
        var noteDeletionRegex = new Regex($"{deleteCommand}\\s{notesSection}\\s\\d");

        if (scheduledMessageDeletionRegex.IsMatch(message))
        {
            var sectionIndex = CalculateSectionIndex(message, scheduledMessagesSection);

            var messageNumber = CalculateSavedObjectNumber(message, sectionIndex);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber,
                    userRecords.ScheduledMessages.Count - 1))
            {
                await ResponseOnBiggerObjectNumber(chatId, localisation, cancellationToken);
            }
            else
            {
                await _timerHandler.RemoveScheduledMessageTimer(userRecords.ScheduledMessages[messageNumber].Id,
                    cancellationToken);
            }
        }

        if (repeatedViaMinutesMessageDeletionRegex.IsMatch(message))
        {
            var sectionIndex = CalculateSectionIndex(message, repeatedViaMinutesMessagesSection);

            var messageNumber = CalculateSavedObjectNumber(message, sectionIndex);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber,
                    userRecords.RepeatedViaMinutesMessages.Count - 1))
            {
                await ResponseOnBiggerObjectNumber(chatId, localisation, cancellationToken);
            }
            else
            {
                await _timerHandler.RemoveRepeatedViaMinutesMessageTimers(
                    userRecords.RepeatedViaMinutesMessages[messageNumber].Id,
                    cancellationToken);
            }
        }

        if (repeatedViaSecondsMessageDeletionRegex.IsMatch(message))
        {
            var sectionIndex = CalculateSectionIndex(message, repeatedViaSecondsMessagesSection);

            var messageNumber = CalculateSavedObjectNumber(message, sectionIndex);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber,
                    userRecords.RepeatedViaSecondsScheduledMessages.Count - 1))
            {
                await ResponseOnBiggerObjectNumber(chatId, localisation, cancellationToken);
            }
            else
            {
                await _timerHandler.RemoveRepeatedViaSecondsMessageTimers(
                    userRecords.RepeatedViaSecondsScheduledMessages[messageNumber].Id,
                    cancellationToken);
            }
        }

        if (noteDeletionRegex.IsMatch(message))
        {
            var sectionIndex = CalculateSectionIndex(message, notesSection);

            var messageNumber = CalculateSavedObjectNumber(message, sectionIndex);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber, userRecords.Notes.Count))
            {
                await ResponseOnBiggerObjectNumber(chatId, localisation, cancellationToken);
            }
            else
            {
                await _noteRepository.Remove(userRecords.Notes[messageNumber].Id, cancellationToken);

                await _unitOfWork.SaveChanges(cancellationToken);
            }
        }
    }

    private int CalculateSectionIndex(string message, string section)
    {
        return message.IndexOf(section, StringComparison.Ordinal);
    }

    private int CalculateSavedObjectNumber(string message, int sectionIndex)
    {
        return Convert.ToInt16(message[(sectionIndex + 2)..]) - 1;
    }

    private bool CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(int messageNumber, int totalAmount)
    {
        return messageNumber > totalAmount - 1;
    }

    private async Task ResponseOnBiggerObjectNumber(long chatId, string localisation,
        CancellationToken cancellationToken)
    {
        var responseMessage =
            _resourceReader.GetString($"{localisation}ObjectNumberIsBiggerThanAmountInSectionMessage");

        await _messageSender.SendMessageToChat(chatId, responseMessage, cancellationToken);
    }
}