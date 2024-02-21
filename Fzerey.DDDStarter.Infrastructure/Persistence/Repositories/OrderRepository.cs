using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Domain.Repositories;
using Fzerey.DDDStarter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Repositories
{
    public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
    {
        public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public void Add(Order order)
        {
            dbContext.Orders.Add(order);
        }
    }
}
