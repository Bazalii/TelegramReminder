namespace NothingToForgetBot.Data.Notes.Models;

public class NoteDbModel
{
    public Guid Id { get; set; }

    public long ChatId { get; set; }

    public string Content { get; set; }
}