using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NothingToForgetBot.Data.Notes.Models;

public class NoteDbModel
{
    public Guid Id { get; set; }

    public long ChatId { get; set; }

    public string Content { get; set; } = string.Empty;
    
    internal class Map : IEntityTypeConfiguration<NoteDbModel>
    {
        public void Configure(EntityTypeBuilder<NoteDbModel> builder)
        {
            builder.ToTable("notes");
            builder.HasKey(dbModel => dbModel.Id).HasName("pk_note");
        }
    }
}