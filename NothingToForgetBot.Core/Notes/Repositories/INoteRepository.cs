using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.Notes.Repositories;

public interface INoteRepository
{
    Task Add(Note note);

    Task<Note> GetById(Guid id);
    
    Task<List<Note>> GetAllByChatId(long chatId);
    
    Task Update(Note note);
    
    Task Remove(Guid id);
}