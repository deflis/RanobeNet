using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class NovelLinkEntityTypeConfiguration : IEntityTypeConfiguration<NovelLink>
    {
        public void Configure(EntityTypeBuilder<NovelLink> builder)
        {
            builder.HasKey(x => new { x.NovelId, x.Link });
            builder.HasOne(x => x.Novel).WithMany(x => x.Links).HasForeignKey(x => x.NovelId).IsRequired();
            builder.Property(x => x.Link).HasMaxLength(255);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
        }
    }
}
