using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class NovelAttributeEntityTypeConfiguration : IEntityTypeConfiguration<NovelAttribute>
    {
        public void Configure(EntityTypeBuilder<NovelAttribute> builder)
        {
            builder.HasOne(x => x.Novel).WithMany(x => x.Attributes).IsRequired();
            builder.HasOne(x => x.Episode);
            builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
        }
    }
}
