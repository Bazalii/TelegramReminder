using System.Resources.NetStandard;
using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Parsers.Implementations;

public class CommandParser : ICommandParser
{
    private readonly string _resourcesFile;

    public CommandParser(string resourcesFile)
    {
        _resourcesFile = resourcesFile;
    }

    public Command Parse(string message)
    {
        var resourceReader = new ResXResourceReader(_resourcesFile);

        if (message == resourceReader.GetString("CommandGuide"))
        {
            return Command.Guide;
        }

        if (message == resourceReader.GetString("CommandLanguage"))
        {
            return Command.Language;
        }

        if (message == resourceReader.GetString("CommandList"))
        {
            return Command.List;
        }

        return Command.NotCommand;
    }
}