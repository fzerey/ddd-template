using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Domain.Repositories;

namespace Fzerey.DDDStarter.Tests.Fakes
{
    internal class InMemoryOrderRepository : IOrderRepository
    {
        public List<Order> Orders { get; } = [];

        public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Orders.FirstOrDefault(o => o.Id == id));
        }

        public void Add(Order order)
        {
            Orders.Add(order);
        }
    }

    internal class InMemoryItemRepository : IItemRepository
    {
        public List<Item> Items { get; } = [];

        public Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.FirstOrDefault(i => i.Id == id));
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }
    }

    internal class FakeUnitOfWork : IUnitOfWork
    {
        public int SaveCount { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveCount++;
            return Task.FromResult(1);
        }
    }
}
