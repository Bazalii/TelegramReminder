using NothingToForgetBot.Core.Commands.Handlers;
using NothingToForgetBot.Core.Commands.Parsers.Implementations;
using NothingToForgetBot.Core.Enums;
using NothingToForgetBot.Core.Exceptions;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Core.Messages.Handlers;
using NothingToForgetBot.Core.Messages.Parsers.Implementations;
using NothingToForgetBot.Core.Notes.Handlers;
using NothingToForgetBot.Core.Notes.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = NothingToForgetBot.Core.Messages.Models.Message;

namespace NothingToForgetBot.Core.ChatActions.UpdateHandling.Implementations;

public class ChatUpdateHandler : IChatUpdateHandler
{
    private readonly CommandParser _commandParser;

    private readonly MessageParser _messageParser;

    private readonly ISupportedLanguagesRepository _supportedLanguagesRepository;

    private readonly ICommandHandler _commandHandler;

    private readonly INoteHandler _noteHandler;

    private readonly IScheduledMessageHandler _scheduledMessageHandler;

    public ChatUpdateHandler(CommandParser commandParser, MessageParser messageParser,
        ISupportedLanguagesRepository supportedLanguagesRepository, ICommandHandler commandHandler,
        INoteHandler noteHandler, IScheduledMessageHandler scheduledMessageHandler)
    {
        _commandParser = commandParser;
        _messageParser = messageParser;
        _supportedLanguagesRepository = supportedLanguagesRepository;
        _commandHandler = commandHandler;
        _noteHandler = noteHandler;
        _scheduledMessageHandler = scheduledMessageHandler;
    }

    public async Task HandleChatUpdate(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;
        if (update.Message!.Type != MessageType.Text)
            return;

        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text ??
                          throw new NullMessageTextValueException("Message text cannot be null!");

        var user = await botClient.GetMeAsync(cancellationToken: cancellationToken);
        var currentLanguage = user.LanguageCode;

        if (currentLanguage == null)
        {
            currentLanguage = "En";
        }
        else
        {
            currentLanguage = currentLanguage[0].ToString().ToUpper() + currentLanguage[1..currentLanguage.Length];

            var supportedLanguages = await _supportedLanguagesRepository.GetAll();

            if (!supportedLanguages.Contains(currentLanguage))
            {
                currentLanguage = "En";
            }
        }

        var command = await Task.Run(() => _commandParser.Parse(messageText), cancellationToken);

        if (command != Command.NotCommand)
        {
            await Task.Run(() => RespondOnCommand(command, chatId), cancellationToken);
            return;
        }

        try
        {
            var message = await Task
                .Run(() => _messageParser.Parse(messageText, currentLanguage), cancellationToken);

            message.Id = Guid.NewGuid();
            message.ChatId = chatId;

            await RespondOnScheduledMessage(message);
        }
        catch (NotScheduledMessageException notScheduledMessageException)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Content = messageText
            };

            await RespondOnNote(note);
        }
    }

    public Task HandlePollingError(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        return Task.Run(() => Console.WriteLine(errorMessage), cancellationToken);
    }

    private Task RespondOnCommand(Command command, long chatId)
    {
        return _commandHandler.Handle(command, chatId);
    }

    private Task RespondOnScheduledMessage(Message message)
    {
        return _scheduledMessageHandler.Handle(message);
    }

    private Task RespondOnNote(Note note)
    {
        return _noteHandler.Handle(note);
    }
}