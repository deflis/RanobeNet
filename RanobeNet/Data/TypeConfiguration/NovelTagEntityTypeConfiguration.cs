using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class NovelTagEntityTypeConfiguration : IEntityTypeConfiguration<NovelTag>
    {
        public void Configure(EntityTypeBuilder<NovelTag> builder)
        {
            builder.HasKey(x => new { x.NovelId, x.Tag });
            builder.HasIndex(x => new { x.Tag, x.NovelId });
            builder.HasOne(x => x.Novel).WithMany(x => x.Tags).IsRequired();
            builder.Property(x => x.Tag).HasMaxLength(100).IsRequired();
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
        }
    }
}
