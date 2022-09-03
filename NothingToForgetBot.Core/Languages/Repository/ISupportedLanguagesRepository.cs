namespace NothingToForgetBot.Core.Languages.Repository;

public interface ISupportedLanguagesRepository
{
    Task Add(string languageName, CancellationToken cancellationToken);
    Task<List<string>> GetAll(CancellationToken cancellationToken);
    Task Remove(string languageName, CancellationToken cancellationToken);
}