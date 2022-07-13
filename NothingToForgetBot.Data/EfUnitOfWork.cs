using NothingToForgetBot.Core;

namespace NothingToForgetBot.Data;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly NothingToForgetBotContext _context;

    public EfUnitOfWork(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}