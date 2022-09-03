namespace NothingToForgetBot.Core.Notes.Models;

public class Note
{
    public Guid Id { get; init; }
    public long ChatId { get; init; }
    public string Content { get; init; } = string.Empty;

    public override string ToString()
    {
        return Content;
    }
}