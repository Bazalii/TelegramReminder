using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Handlers;

public interface ICommandWithArgumentsHandler
{
    Task Handle(CommandWithArguments command, long chatId, string localisation, string message,
        CancellationToken cancellationToken);
}