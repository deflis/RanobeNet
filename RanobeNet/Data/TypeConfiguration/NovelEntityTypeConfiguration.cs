using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class NovelEntityTypeConfiguration : IEntityTypeConfiguration<Novel>
    {
        public void Configure(EntityTypeBuilder<Novel> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
            builder.Property(x => x.Author).HasMaxLength(255);
            builder.HasOne(x => x.User);
            builder.Property(x => x.UserId).IsRequired();
            builder.HasMany(x => x.Chapters).WithOne(x => x.Novel).IsRequired();
            builder.HasMany(x => x.Episodes).WithOne(x => x.Novel).IsRequired();
            builder.HasMany(x => x.Links).WithOne(x => x.Novel).IsRequired();
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
        }
    }
}
