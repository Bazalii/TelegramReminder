namespace NothingToForgetBot.Data.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string message)
        : base(message)
    {
    }
}