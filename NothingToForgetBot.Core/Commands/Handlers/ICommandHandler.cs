﻿using NothingToForgetBot.Core.Enums;

namespace NothingToForgetBot.Core.Commands.Handlers;

public interface ICommandHandler
{
    Task Handle(Command command, long chatId, CancellationToken cancellationToken);
}