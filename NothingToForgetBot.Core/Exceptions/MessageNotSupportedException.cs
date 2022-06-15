namespace NothingToForgetBot.Core.Exceptions;

public class MessageNotSupportedException : Exception
{
    public MessageNotSupportedException(string message)
        : base(message)
    {
    }
}