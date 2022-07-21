using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace NothingToForgetBot.Data.Messages.Models;

public class RepeatedViaMinutesMessageDbModel : MessageDbModel
{
    public int Interval { get; set; }

    public DateTime EndDate { get; set; }
    
    internal class Map : IEntityTypeConfiguration<RepeatedViaMinutesMessageDbModel>
    {
        public void Configure(EntityTypeBuilder<RepeatedViaMinutesMessageDbModel> builder)
        {
            builder.ToTable("repeated_via_minutes_messages");
            builder.HasKey(dbModel => dbModel.Id).HasName("pk_repeated_via_minutes_message");
        }
    }
}