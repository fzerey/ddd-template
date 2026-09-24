using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;

namespace Fzerey.DDDStarter.Application.Common.Interfaces
{
    public interface IItemQueries
    {
        Task<ItemDetailResponse?> GetDetailAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<ItemListResponse>> ListAsync(PageRequest request, CancellationToken cancellationToken = default);
    }
}
