using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RanobeNet.Models.Data;

namespace RanobeNet.Data.TypeConfiguration
{
    public class UserLinkEntityTypeConfiguration : IEntityTypeConfiguration<UserLink>
    {
        public void Configure(EntityTypeBuilder<UserLink> builder)
        {
            builder.HasKey(x => new { x.UserId, x.Link });
            builder.HasOne(x => x.User).WithMany(x => x.Links).IsRequired();
            builder.Property(x => x.Link).HasMaxLength(255);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
        }
    }
}
