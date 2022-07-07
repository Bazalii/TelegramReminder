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

    public async Task<List<Note>> Select(long chatId, CancellationToken cancellationToken)
    {
        var notes = await _noteRepository.GetAllByChatId(chatId, cancellationToken);
        
        notes.Sort((x, y) => string.Compare(x.Content, y.Content, StringComparison.Ordinal));
        
        return notes;
    }
}