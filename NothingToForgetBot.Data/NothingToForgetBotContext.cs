﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NothingToForgetBot.Data.Languages.Models;
using NothingToForgetBot.Data.Messages.Models;
using NothingToForgetBot.Data.Notes.Models;

namespace NothingToForgetBot.Data;

public class NothingToForgetBotContext : DbContext
{
    public DbSet<string> SupportedLanguages { get; set; }

    public DbSet<ChatWithLanguageDbModel> ChatsWithLanguage { get; set; }

    public DbSet<ScheduledMessageDbModel> ScheduledMessages { get; set; }

    public DbSet<RepeatedViaMinutesMessageDbModel> RepeatedViaMinutesMessages { get; set; }

    public DbSet<RepeatedViaSecondsMessageDbModel> RepeatedViaSecondsMessages { get; set; }

    public DbSet<NoteDbModel> Notes { get; set; }

    public NothingToForgetBotContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NothingToForgetBotContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSnakeCaseNamingConvention()
            .UseLazyLoadingProxies();
    }

    public class Factory : IDesignTimeDbContextFactory<NothingToForgetBotContext>
    {
        public NothingToForgetBotContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("FakeConnectionStringOnlyForMigrations")
                .Options;

            return new NothingToForgetBotContext(options);
        }
    }
}