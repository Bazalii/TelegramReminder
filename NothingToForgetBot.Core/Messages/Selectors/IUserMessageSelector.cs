using NothingToForgetBot.Core.Messages.Models;

namespace NothingToForgetBot.Core.Messages.Selectors;

public interface IUserMessageSelector
{
    Task<UserMessages> Select(long chatId, CancellationToken cancellationToken);
}