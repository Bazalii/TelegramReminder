﻿namespace NothingToForgetBot.Core.Messages.Parsers;

public interface IMessageParser
{
    Message Parse(string message, string localisation);
}