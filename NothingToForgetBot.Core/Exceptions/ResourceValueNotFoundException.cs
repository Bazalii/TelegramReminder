namespace NothingToForgetBot.Core.Exceptions;

public class ResourceValueNotFoundException : Exception
{
    public ResourceValueNotFoundException(string message)
        : base(message)
    {
    }
}