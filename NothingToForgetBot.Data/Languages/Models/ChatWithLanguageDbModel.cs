using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NothingToForgetBot.Data.Languages.Models;

public class ChatWithLanguageDbModel
{
    public long ChatId { get; set; }

    public string Language { get; set; }
    
    internal class Map : IEntityTypeConfiguration<ChatWithLanguageDbModel>
    {
        public void Configure(EntityTypeBuilder<ChatWithLanguageDbModel> builder)
        {
            builder.ToTable("chats_with_language");
            builder.HasKey(dbModel => dbModel.ChatId).HasName("pk_chat_with_language");
        }
    }
}