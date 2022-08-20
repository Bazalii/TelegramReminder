using System.Resources.NetStandard;
using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Parsers.Implementations;

public class CommandParser : ICommandParser
{
    private readonly ResXResourceReader _resourceReader;

    public CommandParser(ResXResourceReader resourceReader)
    {
        _resourceReader = resourceReader;
    }

    public Command Parse(string message)
    {
        if (message == _resourceReader.GetString("CommandGuide"))
        {
            return Command.Guide;
        }

        if (message == _resourceReader.GetString("CommandLanguage"))
        {
            return Command.Language;
        }

        if (message == _resourceReader.GetString("CommandList"))
        {
            return Command.List;
        }

        if (message == _resourceReader.GetString("CommandEn") || message == _resourceReader.GetString("CommandRu"))
        {
            return Command.SetLanguage;
        }

        return Command.NotCommand;
    }
}