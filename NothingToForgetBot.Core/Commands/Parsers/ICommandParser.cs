using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Parsers;

public interface ICommandParser
{
    Command Parse(string message);
}