using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NothingToForgetBot.Data.TimeZones.Models;

public class ChatTimeZoneDbModel
{
    public long ChatId { get; set; }
    public int TimeZone { get; set; }
    
    internal class Map : IEntityTypeConfiguration<ChatTimeZoneDbModel>
    {
        public void Configure(EntityTypeBuilder<ChatTimeZoneDbModel> builder)
        {
            builder.ToTable("chat_time_zones");
            builder.HasKey(dbModel => dbModel.ChatId).HasName("pk_chat_time_zone");
        }
    }
}