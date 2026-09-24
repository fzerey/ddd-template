using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Queries
{
    public class ListOrdersQuery : PageRequest, IRequest<PagedResult<OrderListResponse>> { }

    public class ListOrdersQueryHandler(IOrderQueries orderQueries)
        : IRequestHandler<ListOrdersQuery, PagedResult<OrderListResponse>>
    {
        public Task<PagedResult<OrderListResponse>> Handle(
            ListOrdersQuery request,
            CancellationToken cancellationToken
        )
        {
            return orderQueries.ListAsync(request, cancellationToken);
        }
    }
}
