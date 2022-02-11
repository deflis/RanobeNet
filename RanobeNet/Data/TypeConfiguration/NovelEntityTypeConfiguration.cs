using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class NovelEntityTypeConfiguration : IEntityTypeConfiguration<Novel>
    {
        public void Configure(EntityTypeBuilder<Novel> builder)
        {
            builder.HasOne(x => x.User);
            builder.Property(x => x.UserId).IsRequired();
            builder.HasMany(x => x.Chapters).WithOne(x => x.Novel).IsRequired();
            builder.HasMany(x => x.Episodes).WithOne(x => x.Novel).IsRequired();
        }
    }
}
