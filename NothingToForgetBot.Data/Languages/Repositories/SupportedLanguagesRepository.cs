using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Languages.Repository;

namespace NothingToForgetBot.Data.Languages.Repositories;

public class SupportedLanguagesRepository : ISupportedLanguagesRepository
{
    private readonly NothingToForgetBotContext _context;

    public SupportedLanguagesRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(string language, CancellationToken cancellationToken)
    {
        await _context.SupportedLanguages.AddAsync(language, cancellationToken);
    }

    public Task<List<string>> GetAll(CancellationToken cancellationToken)
    {
        return _context.SupportedLanguages.ToListAsync(cancellationToken);
    }

    public Task Remove(string language, CancellationToken cancellationToken)
    {
        _context.SupportedLanguages.Remove(language);
        
        return Task.CompletedTask;
    }
}