namespace NothingToForgetBot.Core.Exceptions;

public class NullResourceValueException : Exception
{
    public NullResourceValueException(string message)
        : base(message)
    {
    }
}