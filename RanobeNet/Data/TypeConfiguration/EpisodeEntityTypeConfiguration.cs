using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class EpisodeEntityTypeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.HasOne(x => x.Novel).WithMany(x => x.Episodes).IsRequired();
            builder.HasOne(x => x.Chapter).WithMany(x => x.Episodes);
        }
    }
}
