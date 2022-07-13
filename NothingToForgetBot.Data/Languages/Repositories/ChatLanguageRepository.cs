using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core.Languages;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Languages.Models;

namespace NothingToForgetBot.Data.Languages.Repositories;

public class ChatLanguageRepository : IChatLanguageRepository
{
    private readonly NothingToForgetBotContext _context;

    public ChatLanguageRepository(NothingToForgetBotContext context)
    {
        _context = context;
    }

    public async Task Add(ChatWithLanguage chatWithLanguage, CancellationToken cancellationToken)
    {
        await _context.ChatsWithLanguage.AddAsync(new ChatWithLanguageDbModel
        {
            ChatId = chatWithLanguage.ChatId,
            Language = chatWithLanguage.Language
        }, cancellationToken);
    }

    public async Task<string> GetLanguageByChatId(long chatId, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ChatsWithLanguage
            .AsNoTracking()
            .FirstOrDefaultAsync(chatWithLanguage => chatWithLanguage.ChatId == chatId, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Chat with id: {chatId} is not found!");
        }

        return dbModel.Language;
    }

    public async Task Update(ChatWithLanguage chatWithLanguage, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ChatsWithLanguage
            .FirstOrDefaultAsync(chatWithLanguageDbModel => chatWithLanguageDbModel.ChatId == chatWithLanguage.ChatId,
                cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Chat with id: {chatWithLanguage.ChatId} is not found!");
        }

        dbModel.Language = chatWithLanguage.Language;
    }

    public async Task RemoveByChatId(long chatId, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ChatsWithLanguage
            .FirstOrDefaultAsync(chatWithLanguage => chatWithLanguage.ChatId == chatId, cancellationToken);

        if (dbModel is null)
        {
            throw new ObjectNotFoundException($"Chat with id: {chatId} is not found!");
        }

        _context.ChatsWithLanguage.Remove(dbModel);
    }
}