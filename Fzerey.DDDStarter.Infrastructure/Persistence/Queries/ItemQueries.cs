using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using Fzerey.DDDStarter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Queries
{
    public class ItemQueries(ApplicationDbContext dbContext) : IItemQueries
    {
        public Task<ItemDetailResponse?> GetDetailAsync(int id, CancellationToken cancellationToken = default)
        {
            return dbContext.Items
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new ItemDetailResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<PagedResult<ItemListResponse>> ListAsync(PageRequest request, CancellationToken cancellationToken = default)
        {
            return dbContext.Items
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .Select(i => new ItemListResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price
                })
                .ToPagedResultAsync(request, cancellationToken);
        }
    }
}
