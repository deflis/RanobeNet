using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RanobeNet.Data.TypeConfiguration;
using RanobeNet.Models.Data;

namespace RanobeNet.Data
{
    public class RanobeNetContext : DbContext
    {
        public RanobeNetContext(DbContextOptions<RanobeNetContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserLinkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NovelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NovelAttributeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NovelLinkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NovelTagEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EpisodeEntityTypeConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLink> UserLinks { get; set; }
        public DbSet<Novel> Novels { get; set; }
        public DbSet<NovelAttribute> NovelAttributes { get; set; }
        public DbSet<NovelLink> NovelLinks { get; set; }
        public DbSet<NovelTag> NovelTags { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
    }
    internal static class DbEntityExtension
    {
        public static PropertyEntry? SafeGetProperty(this EntityEntry entry, string propertyName)
        {
            if (entry.Metadata.FindProperty(propertyName) != null)
            {
                return entry.Property(propertyName);
            }

            return null;
        }
    }
}
