namespace NothingToForgetBot.Core.Languages.Repository;

public interface IChatLanguageRepository
{
    Task Add(ChatWithLanguage chatWithLanguage, CancellationToken cancellationToken);

    Task<string> GetLanguageByChatIdOrDefault(long chatId, CancellationToken cancellationToken);

    Task Update(ChatWithLanguage chatWithLanguage, CancellationToken cancellationToken);

    Task RemoveByChatId(long chatId, CancellationToken cancellationToken);
}