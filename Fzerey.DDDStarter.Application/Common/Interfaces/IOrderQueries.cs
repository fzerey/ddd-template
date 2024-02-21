using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;

namespace Fzerey.DDDStarter.Application.Common.Interfaces
{
    public interface IOrderQueries
    {
        Task<OrderDetailResponse?> GetDetailAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<OrderListResponse>> ListAsync(PageRequest request, CancellationToken cancellationToken = default);
    }
}
