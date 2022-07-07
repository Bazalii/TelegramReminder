using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.Notes.Repositories;

public interface INoteRepository
{
    Task Add(Note note, CancellationToken cancellationToken);

    Task<Note> GetById(Guid id, CancellationToken cancellationToken);

    Task<List<Note>> GetAllByChatId(long chatId, CancellationToken cancellationToken);

    Task Update(Note note, CancellationToken cancellationToken);

    Task Remove(Guid id, CancellationToken cancellationToken);
}