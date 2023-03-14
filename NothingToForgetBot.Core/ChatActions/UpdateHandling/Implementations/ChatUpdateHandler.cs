using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.Commands.Handlers;
using NothingToForgetBot.Core.Commands.Parsers;
using NothingToForgetBot.Core.Enums;
using NothingToForgetBot.Core.Exceptions;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Core.Messages.Handlers;
using NothingToForgetBot.Core.Messages.Parsers;
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
    private readonly ICommandParser _commandParser;
    private readonly ICommandWithArgumentsParser _commandWithArgumentsParser;
    private readonly IMessageParser _messageParser;
    private readonly ISupportedLanguagesRepository _supportedLanguagesRepository;
    private readonly ICommandHandler _commandHandler;
    private readonly ICommandWithArgumentsHandler _commandWithArgumentsHandler;
    private readonly INoteHandler _noteHandler;
    private readonly IScheduledMessageHandler _scheduledMessageHandler;
    private readonly IChatLanguageRepository _chatLanguageRepository;
    private readonly IMessageSender _messageSender;

    public ChatUpdateHandler(ICommandParser commandParser, ICommandWithArgumentsParser commandWithArgumentsParser,
        IMessageParser messageParser, ISupportedLanguagesRepository supportedLanguagesRepository,
        ICommandHandler commandHandler, ICommandWithArgumentsHandler commandWithArgumentsHandler,
        INoteHandler noteHandler, IScheduledMessageHandler scheduledMessageHandler,
        IChatLanguageRepository chatLanguageRepository, IMessageSender messageSender)
    {
        _commandParser = commandParser;
        _commandWithArgumentsParser = commandWithArgumentsParser;
        _messageParser = messageParser;
        _supportedLanguagesRepository = supportedLanguagesRepository;
        _commandHandler = commandHandler;
        _commandWithArgumentsHandler = commandWithArgumentsHandler;
        _noteHandler = noteHandler;
        _scheduledMessageHandler = scheduledMessageHandler;
        _chatLanguageRepository = chatLanguageRepository;
        _messageSender = messageSender;
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

        var currentLanguage = await _chatLanguageRepository.GetLanguageByChatIdOrDefault(chatId, cancellationToken);

        var command = _commandParser.Parse(messageText);

        if (command != Command.NotCommand)
        {
            await RespondOnCommand(command, chatId, messageText, cancellationToken);
            return;
        }

        var commandWithArguments = _commandWithArgumentsParser.Parse(messageText);

        if (commandWithArguments != CommandWithArguments.NotCommandWithArguments)
        {
            await RespondOnCommandWithArguments(commandWithArguments, chatId, currentLanguage, messageText,
                cancellationToken);
            return;
        }

        try
        {
            var message = await _messageParser.Parse(chatId, messageText, currentLanguage, cancellationToken);

            message.Id = Guid.NewGuid();
            message.ChatId = chatId;

            await RespondOnScheduledMessage(message, cancellationToken);
        }
        catch (NotScheduledMessageException)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Content = messageText
            };

            await RespondOnNote(note, cancellationToken);
        }
        catch (DateIsEarlierThanNowException dateIsEarlierThanNowException)
        {
            await _messageSender.SendMessageToChat(chatId, dateIsEarlierThanNowException.Message, cancellationToken);
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

    private Task RespondOnCommand(Command command, long chatId, string message,
        CancellationToken cancellationToken)
    {
        return _commandHandler.Handle(command, chatId, message, cancellationToken);
    }

    private Task RespondOnCommandWithArguments(CommandWithArguments command, long chatId, string localisation,
        string message, CancellationToken cancellationToken)
    {
        return _commandWithArgumentsHandler.Handle(command, chatId, localisation, message, cancellationToken);
    }

    private Task RespondOnScheduledMessage(Message message, CancellationToken cancellationToken)
    {
        return _scheduledMessageHandler.Handle(message, cancellationToken);
    }

    private Task RespondOnNote(Note note, CancellationToken cancellationToken)
    {
        return _noteHandler.Handle(note, cancellationToken);
    }
}