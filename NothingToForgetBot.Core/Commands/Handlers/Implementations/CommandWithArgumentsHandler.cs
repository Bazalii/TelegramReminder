using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.ChatActions.RecordSelectors;
using NothingToForgetBot.Core.Enums;
using NothingToForgetBot.Core.Exceptions;
using NothingToForgetBot.Core.Notes.Repositories;
using NothingToForgetBot.Core.Timers.Handlers;
using NothingToForgetBot.Core.TimeZones.Models;
using NothingToForgetBot.Core.TimeZones.Repositories;

namespace NothingToForgetBot.Core.Commands.Handlers.Implementations;

public class CommandWithArgumentsHandler : ICommandWithArgumentsHandler
{
    private readonly ITimerHandler _timerHandler;
    private readonly INoteRepository _noteRepository;
    private readonly ITimeZoneRepository _timeZoneRepository;
    private readonly ResXResourceReader _resourceReader;
    private readonly IUserRecordSelector _userRecordSelector;
    private readonly IMessageSender _messageSender;
    private readonly IUnitOfWork _unitOfWork;

    public CommandWithArgumentsHandler(ITimerHandler timerHandler, INoteRepository noteRepository,
        ITimeZoneRepository timeZoneRepository, ResXResourceReader resourceReader,
        IUserRecordSelector userRecordSelector, IMessageSender messageSender, IUnitOfWork unitOfWork)
    {
        _timerHandler = timerHandler;
        _noteRepository = noteRepository;
        _timeZoneRepository = timeZoneRepository;
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
            case CommandWithArguments.SetTimeZone:
                await HandleSetTimeZoneCommand(chatId, message, cancellationToken);
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
            var indexOfSectionEnd = CalculateIndexOfSectionEnd(message, scheduledMessagesSection);

            var messageNumber = CalculateSavedObjectNumber(message, indexOfSectionEnd);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber,
                    userRecords.ScheduledMessages.Count))
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
            var indexOfSectionEnd = CalculateIndexOfSectionEnd(message, repeatedViaMinutesMessagesSection);

            var messageNumber = CalculateSavedObjectNumber(message, indexOfSectionEnd);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber,
                    userRecords.RepeatedViaMinutesMessages.Count))
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
            var indexOfSectionEnd = CalculateIndexOfSectionEnd(message, repeatedViaSecondsMessagesSection);

            var messageNumber = CalculateSavedObjectNumber(message, indexOfSectionEnd);

            if (CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(messageNumber,
                    userRecords.RepeatedViaSecondsMessages.Count))
            {
                await ResponseOnBiggerObjectNumber(chatId, localisation, cancellationToken);
            }
            else
            {
                await _timerHandler.RemoveRepeatedViaSecondsMessageTimers(
                    userRecords.RepeatedViaSecondsMessages[messageNumber].Id,
                    cancellationToken);
            }
        }

        if (noteDeletionRegex.IsMatch(message))
        {
            var indexOfSectionEnd = CalculateIndexOfSectionEnd(message, notesSection);

            var messageNumber = CalculateSavedObjectNumber(message, indexOfSectionEnd);

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

    private async Task HandleSetTimeZoneCommand(long chatId, string message, CancellationToken cancellationToken)
    {
        var setTimeZoneCommand = _resourceReader.GetString("CommandSetTimeZone");

        var timeZone = Convert.ToInt16(message[(setTimeZoneCommand.Length + 1)..]);

        var chatWithTimeZone = new ChatTimeZone
        {
            ChatId = chatId,
            TimeZone = timeZone
        };

        try
        {
            await _timeZoneRepository.Add(chatWithTimeZone, cancellationToken);
        }
        catch (ObjectAlreadyExistsException)
        {
            await _timeZoneRepository.Update(chatWithTimeZone, cancellationToken);
        }

        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private static int CalculateIndexOfSectionEnd(string message, string section)
    {
        return message.IndexOf(section, StringComparison.Ordinal) + section.Length;
    }

    private static int CalculateSavedObjectNumber(string message, int indexOfSectionEnd)
    {
        return Convert.ToInt16(message[indexOfSectionEnd..]) - 1;
    }

    private static bool CheckIfNumberIsBiggerThanSectionObjectsTotalAmount(int messageNumber, int totalAmount)
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