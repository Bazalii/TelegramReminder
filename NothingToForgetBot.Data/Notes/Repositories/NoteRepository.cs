using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Notes.Models;
using NothingToForgetBot.Core.Notes.Repositories;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Notes.Models;

namespace NothingToForgetBot.Data.Notes.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NothingToForgetBotContext _context;

    public NoteRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(Note note, CancellationToken cancellationToken)
    {
        await _context.Notes.AddAsync(new NoteDbModel
        {
            Id = note.Id,
            ChatId = note.ChatId,
            Content = note.Content
        }, cancellationToken);
    }

    public async Task<Note> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(note => note.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Note with id: {id} is not found!");
        }

        return new Note
        {
            Id = dbModel.Id,
            ChatId = dbModel.ChatId,
            Content = dbModel.Content
        };
    }

    public async Task<List<Note>> GetAllByChatId(long chatId, CancellationToken cancellationToken)
    {
        return await _context.Notes
            .AsNoTracking()
            .Select(note => new Note
            {
                Id = note.Id,
                ChatId = note.ChatId,
                Content = note.Content,
            })
            .Where(note => note.ChatId == chatId)
            .ToListAsync(cancellationToken);
    }

    public async Task Update(Note note, CancellationToken cancellationToken)
    {
        var dbModel =
            await _context.Notes.FirstOrDefaultAsync(noteDbModel => noteDbModel.Id == note.Id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Note with id: {note.Id} is not found!");
        }

        dbModel.ChatId = note.ChatId;
        dbModel.Content = note.Content;
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Notes.FirstOrDefaultAsync(note => note.Id == id, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Note with id: {id} is not found!");
        }

        _context.Notes.Remove(dbModel);
    }
}