namespace NothingToForgetBot.Core.Exceptions;

public class NotScheduledMessageException : Exception
{
    public NotScheduledMessageException(string message)
        : base(message)
    {
    }
}