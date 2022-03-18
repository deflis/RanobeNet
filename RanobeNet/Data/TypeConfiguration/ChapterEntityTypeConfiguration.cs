using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class ChapterEntityTypeConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.HasOne(x => x.Novel).WithMany(x => x.Chapters).IsRequired();
            builder.HasMany(x => x.Episodes).WithOne(x => x.Chapter);
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
        }
    }
}
