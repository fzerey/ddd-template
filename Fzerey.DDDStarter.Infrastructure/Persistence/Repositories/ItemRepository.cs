using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Domain.Repositories;
using Fzerey.DDDStarter.Infrastructure.Context;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Repositories
{
    public class ItemRepository(ApplicationDbContext dbContext) : IItemRepository
    {
        public async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Items.FindAsync([id], cancellationToken);
        }

        public void Add(Item item)
        {
            dbContext.Items.Add(item);
        }
    }
}
