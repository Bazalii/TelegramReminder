namespace NothingToForgetBot.Core.Exceptions;

public class NullMessageTextValueException : Exception
{
    public NullMessageTextValueException(string message)
        : base(message)
    {
    }
}