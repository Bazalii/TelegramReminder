using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Parsers;

public interface ICommandParser
{
    public Command Parse(string message);
}