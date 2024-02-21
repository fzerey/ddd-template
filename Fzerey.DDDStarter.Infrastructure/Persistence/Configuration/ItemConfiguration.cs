using Fzerey.DDDStarter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Configuration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).HasMaxLength(100).IsRequired();
            builder.Property(o => o.Price).IsRequired();

        }
    }
}
