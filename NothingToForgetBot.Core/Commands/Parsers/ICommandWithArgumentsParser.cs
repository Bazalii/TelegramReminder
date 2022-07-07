using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Parsers;

public interface ICommandWithArgumentsParser
{
    CommandWithArguments Parse(string message);
}