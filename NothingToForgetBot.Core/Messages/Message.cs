namespace NothingToForgetBot.Core.Messages;

public abstract class Message
{
    public Guid Id { get; set; }
    
    public long ChatId { get; set; }
    public string Content { get; set; }
}