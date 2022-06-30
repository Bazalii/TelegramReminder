namespace NothingToForgetBot.Core.Notes.Models;

public class Note
{
    public Guid Id { get; set; }
    
    public long ChatId { get; set; }
    
    public string Content { get; set; }

    public override string ToString()
    {
        return Content;
    }
}