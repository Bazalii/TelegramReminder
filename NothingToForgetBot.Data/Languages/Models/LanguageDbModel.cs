using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NothingToForgetBot.Data.Languages.Models;

public class LanguageDbModel
{
    public string Name { get; init; } = string.Empty;
    
    internal class Map : IEntityTypeConfiguration<LanguageDbModel>
    {
        public void Configure(EntityTypeBuilder<LanguageDbModel> builder)
        {
            builder.ToTable("supported_languages");
            builder.HasNoKey();
        }
    }
}