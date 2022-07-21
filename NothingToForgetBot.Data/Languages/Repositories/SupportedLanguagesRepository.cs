using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Data.Languages.Models;

namespace NothingToForgetBot.Data.Languages.Repositories;

public class SupportedLanguagesRepository : ISupportedLanguagesRepository
{
    private readonly NothingToForgetBotContext _context;

    public SupportedLanguagesRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(string languageName, CancellationToken cancellationToken)
    {
        var language = new LanguageDbModel
        {
            Name = languageName
        };
        
        await _context.SupportedLanguages.AddAsync(language, cancellationToken);
    }

    public Task<List<string>> GetAll(CancellationToken cancellationToken)
    {
        return _context.SupportedLanguages
            .Select(language => language.Name)
            .ToListAsync(cancellationToken);
    }

    public Task Remove(string languageName, CancellationToken cancellationToken)
    {
        var language = new LanguageDbModel
        {
            Name = languageName
        };
        
        _context.SupportedLanguages.Remove(language);

        return Task.CompletedTask;
    }
}