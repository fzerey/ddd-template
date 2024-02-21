using Fzerey.DDDStarter.Domain.Model;

namespace Fzerey.DDDStarter.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(Order order);
    }
}
