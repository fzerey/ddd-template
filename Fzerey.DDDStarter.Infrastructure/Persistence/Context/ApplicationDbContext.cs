using System.Reflection;
using Fzerey.DDDStarter.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fzerey.DDDStarter.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
