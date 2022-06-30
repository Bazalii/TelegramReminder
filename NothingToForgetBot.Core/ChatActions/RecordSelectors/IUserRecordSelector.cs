using NothingToForgetBot.Core.ChatActions.RecordSelectors.Models;

namespace NothingToForgetBot.Core.ChatActions.RecordSelectors;

public interface IUserRecordSelector
{
    Task<UserRecords> Select(long chatId, CancellationToken cancellationToken);
}