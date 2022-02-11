using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class ChapterEntityTypeConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasOne(x => x.Novel).WithMany(x => x.Chapters).IsRequired();
            builder.HasMany(x => x.Episodes).WithOne(x => x.Chapter);
        }
    }
}
