using NothingToForgetBot.Core.Notes.Models;
using NothingToForgetBot.Core.Notes.Repositories;

namespace NothingToForgetBot.Core.Notes.Handlers.Implementations;

public class NoteHandler : INoteHandler
{
    private readonly INoteRepository _noteRepository;

    private readonly IUnitOfWork _unitOfWork;

    public NoteHandler(INoteRepository noteRepository, IUnitOfWork unitOfWork)
    {
        _noteRepository = noteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(Note note, CancellationToken cancellationToken)
    {
        await _noteRepository.Add(note, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
    }
}