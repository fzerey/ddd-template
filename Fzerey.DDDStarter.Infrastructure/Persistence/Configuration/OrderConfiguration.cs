using Fzerey.DDDStarter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fzerey.DDDStarter.Infrastructure.Context;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.CustomerName).HasMaxLength(64).IsRequired();
    }
}