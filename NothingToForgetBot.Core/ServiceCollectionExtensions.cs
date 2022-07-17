using System.Resources.NetStandard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse;
using NothingToForgetBot.Core.ChatActions.ChatResponse.MessageResponse.Implementations;
using NothingToForgetBot.Core.ChatActions.RecordSelectors;
using NothingToForgetBot.Core.ChatActions.RecordSelectors.Implementations;
using NothingToForgetBot.Core.ChatActions.UpdateHandling;
using NothingToForgetBot.Core.ChatActions.UpdateHandling.Implementations;
using NothingToForgetBot.Core.Commands.Handlers;
using NothingToForgetBot.Core.Commands.Handlers.Implementations;
using NothingToForgetBot.Core.Commands.Parsers;
using NothingToForgetBot.Core.Commands.Parsers.Implementations;
using NothingToForgetBot.Core.Messages.Handlers;
using NothingToForgetBot.Core.Messages.Handlers.Implementations;
using NothingToForgetBot.Core.Messages.Parsers;
using NothingToForgetBot.Core.Messages.Parsers.Implementations;
using NothingToForgetBot.Core.Messages.Selectors;
using NothingToForgetBot.Core.Messages.Selectors.Implementations;
using NothingToForgetBot.Core.Notes.Handlers;
using NothingToForgetBot.Core.Notes.Handlers.Implementations;
using NothingToForgetBot.Core.Notes.Selectors;
using NothingToForgetBot.Core.Notes.Selectors.Implementations;
using NothingToForgetBot.Core.Timers.Handlers;
using NothingToForgetBot.Core.Timers.Handlers.Implementations;

namespace NothingToForgetBot.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IMessageSender, MessageSender>()
            .AddSingleton<IUserRecordSelector, UserRecordSelector>()
            .AddSingleton<ICommandHandler, CommandHandler>()
            .AddSingleton<ICommandWithArgumentsHandler, CommandWithArgumentsHandler>()
            .AddSingleton<ICommandParser, CommandParser>()
            .AddSingleton<ICommandWithArgumentsParser, CommandWithArgumentsParser>()
            .AddSingleton<IScheduledMessageHandler, ScheduledMessageHandler>()
            .AddSingleton<IMessageParser, MessageParser>()
            .AddSingleton<IUserMessageSelector, UserMessageSelector>()
            .AddSingleton<INoteHandler, NoteHandler>()
            .AddSingleton<IUserNoteSelector, UserNoteSelector>()
            .AddSingleton<ITimerHandler, TimerHandler>()
            .AddSingleton<ResXResourceReader>(_ => new ResXResourceReader(configuration["ResourcesPath"]))
            .AddSingleton<IChatUpdateHandler, ChatUpdateHandler>();

        return services;
    }
}