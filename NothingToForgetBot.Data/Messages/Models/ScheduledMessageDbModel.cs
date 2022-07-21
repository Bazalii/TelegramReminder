using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NothingToForgetBot.Data.Messages.Models;

public class ScheduledMessageDbModel : MessageDbModel
{
    public DateTime PublishingDate { get; set; }
    
    internal class Map : IEntityTypeConfiguration<ScheduledMessageDbModel>
    {
        public void Configure(EntityTypeBuilder<ScheduledMessageDbModel> builder)
        {
            builder.ToTable("scheduled_messages");
            builder.HasKey(dbModel => dbModel.Id).HasName("pk_scheduled_message");
        }
    }
}