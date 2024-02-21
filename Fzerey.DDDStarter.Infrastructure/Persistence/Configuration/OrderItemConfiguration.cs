using Fzerey.DDDStarter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Quantity).IsRequired();
            builder.Property(o => o.ItemId).IsRequired();
            builder.Property(o => o.OrderId).IsRequired();
            builder.HasOne(o => o.Item).WithMany().HasForeignKey(o => o.ItemId);
            builder.HasOne(o => o.Order).WithMany(o => o.OrderItems).HasForeignKey(o => o.OrderId);
        }
    }
}
