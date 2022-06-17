namespace NothingToForgetBot.Core.Notes;

public class Note
{
    public Guid Id { get; set; }
    
    public long ChatId { get; set; }
    
    public string Content { get; set; }
}