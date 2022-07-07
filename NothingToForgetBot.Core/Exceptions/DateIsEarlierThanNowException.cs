namespace NothingToForgetBot.Core.Exceptions;

public class DateIsEarlierThanNowException : Exception
{
    public DateIsEarlierThanNowException(string message)
        : base(message)
    {
    }
}