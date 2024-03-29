﻿using System.Resources.NetStandard;
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
        var deleteCommand = _resourceReader.GetString("CommandDelete");
        var scheduledMessagesSection = _resourceReader.GetString("ScheduledMessagesSection");
        var repeatedViaMinutesMessagesSection = _resourceReader.GetString("RepeatedViaMinutesMessagesSection");
        var repeatedViaSecondsMessagesSection = _resourceReader.GetString("RepeatedViaSecondsMessagesSection");
        var notesSection = _resourceReader.GetString("NotesSection");
        var setTimeZoneCommand = _resourceReader.GetString("CommandSetTimeZone");

        var deleteCommandRegex =
            new Regex($"{deleteCommand}\\s({scheduledMessagesSection}|{repeatedViaMinutesMessagesSection}" +
                      $"|{repeatedViaSecondsMessagesSection}|{notesSection})\\s\\d");

        var setTimeZoneCommandRegex = new Regex($"{setTimeZoneCommand}\\s\\d");
        
        if (deleteCommandRegex.IsMatch(message))
        {
            return CommandWithArguments.Delete;
        }
        
        if (setTimeZoneCommandRegex.IsMatch(message))
        {
            return CommandWithArguments.SetTimeZone;
        }

        return CommandWithArguments.NotCommandWithArguments;
    }
}