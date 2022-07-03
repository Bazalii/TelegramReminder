using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Parsers.Implementations;

public class CommandWithArgumentsParser : ICommandWithArgumentsParser
{
    private readonly ResXResourceReader _resourceReader;

    public CommandWithArgumentsParser(ResXResourceReader resourceReader)
    {
        _resourceReader = resourceReader;
    }

    public CommandWithArguments Parse(string message)
    {
        var deleteCommandResourceValue = _resourceReader.GetString("CommandDelete");
        
        var deleteCommandRegex = new Regex($"{deleteCommandResourceValue}\\s\\d");

        if (deleteCommandRegex.IsMatch(message))
        {
            return CommandWithArguments.Delete;
        }

        return CommandWithArguments.NotCommandWithArguments;
    }
}