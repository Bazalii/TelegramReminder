namespace NothingToForgetBot.Core.Languages.Repository;

public interface ISupportedLanguagesRepository
{
    Task Add(string language);
    
    Task<List<string>> GetAll();

    Task Remove(string language);
}