using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NothingToForgetBot.Data.Messages.Models;

public class RepeatedViaSecondsMessageDbModel : MessageDbModel
{
    public int Interval { get; set; }
    public DateTime EndDate { get; set; }
    
    internal class Map : IEntityTypeConfiguration<RepeatedViaSecondsMessageDbModel>
    {
        public void Configure(EntityTypeBuilder<RepeatedViaSecondsMessageDbModel> builder)
        {
            builder.ToTable("repeated_via_seconds_messages");
            builder.HasKey(dbModel => dbModel.Id).HasName("pk_repeated_via_seconds_message");
        }
    }
}