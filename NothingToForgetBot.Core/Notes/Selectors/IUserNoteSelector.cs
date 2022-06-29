using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.Notes.Selectors;

public interface IUserNoteSelector
{
    Task<List<Note>> Select(long chatId, CancellationToken cancellationToken);
}