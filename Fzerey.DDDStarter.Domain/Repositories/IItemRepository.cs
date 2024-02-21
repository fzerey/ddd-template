using Fzerey.DDDStarter.Domain.Model;

namespace Fzerey.DDDStarter.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(Item item);
    }
}
