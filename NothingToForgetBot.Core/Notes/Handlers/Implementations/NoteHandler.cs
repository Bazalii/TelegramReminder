using NothingToForgetBot.Core.Notes.Models;
using NothingToForgetBot.Core.Notes.Repositories;

namespace NothingToForgetBot.Core.Notes.Handlers.Implementations;

public class NoteHandler : INoteHandler
{
    private readonly INoteRepository _noteRepository;

    public NoteHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }
    
    public Task Handle(Note note)
    {
        return _noteRepository.Add(note);
    }
}