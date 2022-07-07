namespace NothingToForgetBot.Core;

public interface IUnitOfWork
{
    Task<int> SaveChanges(CancellationToken cancellationToken);
}