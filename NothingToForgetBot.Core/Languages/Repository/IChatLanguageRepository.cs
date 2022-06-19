namespace NothingToForgetBot.Core.Languages.Repository;

public interface IChatLanguageRepository
{
    Task Add(ChatWithLanguage chatWithLanguage);

    Task<string> GetLanguageByChatId(long chatId);
    
    Task Update(ChatWithLanguage chatWithLanguage);
    
    Task RemoveByChatId(long chatId);
}