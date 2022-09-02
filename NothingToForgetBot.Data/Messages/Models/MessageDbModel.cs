namespace NothingToForgetBot.Data.Messages.Models;

public abstract class MessageDbModel
{
    public Guid Id { get; init; }
    public long ChatId { get; set; }
    public string Content { get; set; } = string.Empty;
}