using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingToForgetBot.Core;
using NothingToForgetBot.Core.Languages.Repository;
using NothingToForgetBot.Core.Messages.Repositories;
using NothingToForgetBot.Core.Notes.Repositories;
using NothingToForgetBot.Data.Languages.Repositories;
using NothingToForgetBot.Data.Messages.Repositories;
using NothingToForgetBot.Data.Notes.Repositories;

namespace NothingToForgetBot.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IChatLanguageRepository, ChatLanguageRepository>()
            .AddSingleton<ISupportedLanguagesRepository, SupportedLanguagesRepository>()
            .AddSingleton<IScheduledMessageRepository, ScheduledMessageRepository>()
            .AddSingleton<IRepeatedViaMinutesMessageRepository, RepeatedViaMinutesMessageRepository>()
            .AddSingleton<IRepeatedViaSecondsMessageRepository, RepeatedViaSecondsMessageRepository>()
            .AddSingleton<INoteRepository, NoteRepository>()
            .AddSingleton<IUnitOfWork, EfUnitOfWork>()
            .AddDbContext<NothingToForgetBotContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(configuration["DbConnectionString"]));

        return services;
    }
}