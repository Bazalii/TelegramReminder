namespace NothingToForgetBot.Core.Exceptions;

public class ObjectAlreadyExistsException : Exception
{
    public ObjectAlreadyExistsException(string message)
        : base(message)
    {
    }
}