using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Parsers;

public interface IMessageParser
{
    Task<Message> Parse(long chatId, string message, string localisation, CancellationToken cancellationToken);
}