namespace NothingToForgetBot.Core.Languages.Repository;

public interface ISupportedLanguagesRepository
{
    Task Add(string language, CancellationToken cancellationToken);

    Task<List<string>> GetAll(CancellationToken cancellationToken);

    Task Remove(string language, CancellationToken cancellationToken);
}