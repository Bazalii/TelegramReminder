namespace NothingToForgetBot.Core.Exceptions;

public class TimerNotExistsException : Exception
{
    public TimerNotExistsException(string message)
        : base(message)
    {
    }
}