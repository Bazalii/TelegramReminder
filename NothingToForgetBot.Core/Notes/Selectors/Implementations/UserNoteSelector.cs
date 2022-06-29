using NothingToForgetBot.Core.Notes.Models;
using NothingToForgetBot.Core.Notes.Repositories;

namespace NothingToForgetBot.Core.Notes.Selectors.Implementations;

public class UserNoteSelector : IUserNoteSelector
{
    private readonly INoteRepository _noteRepository;

    public UserNoteSelector(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public Task<List<Note>> Select(long chatId, CancellationToken cancellationToken)
    {
        return _noteRepository.GetAllByChatId(chatId, cancellationToken);
    }
}