using System.Resources.NetStandard;
using Microsoft.EntityFrameworkCore;
using NothingToForgetBot.Core;
using NothingToForgetBot.Core.Languages;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Data.Exceptions;
using NothingToForgetBot.Data.Languages.Models;

namespace NothingToForgetBot.Data.Languages.Repositories;

public class ChatLanguageRepository : IChatLanguageRepository
{
    private readonly NothingToForgetBotContext _context;

    private readonly ResXResourceReader _resourceReader;

    public ChatLanguageRepository(NothingToForgetBotContext context, ResXResourceReader resourceReader)
    {
        _context = context;
        _resourceReader = resourceReader;
    }

    public async Task Add(ChatWithLanguage chatWithLanguage, CancellationToken cancellationToken)
    {
        await _context.ChatsWithLanguage.AddAsync(new ChatWithLanguageDbModel
        {
            ChatId = chatWithLanguage.ChatId,
            Language = chatWithLanguage.Language
        }, cancellationToken);
    }

    public async Task<string?> GetLanguageByChatId(long chatId, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ChatsWithLanguage
            .AsNoTracking()
            .FirstOrDefaultAsync(chatWithLanguage => chatWithLanguage.ChatId == chatId, cancellationToken);

        return dbModel?.Language;
    }
    
    public async Task<string> GetLanguageByChatIdOrDefault(long chatId, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ChatsWithLanguage
            .AsNoTracking()
            .FirstOrDefaultAsync(chatWithLanguage => chatWithLanguage.ChatId == chatId, cancellationToken);

        return dbModel is null ? _resourceReader.GetString("DefaultLanguage") : dbModel.Language;
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