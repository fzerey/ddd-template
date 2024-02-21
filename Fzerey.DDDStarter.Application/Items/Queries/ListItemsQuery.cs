using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Queries
{
    public class ListItemsQuery : PageRequest, IRequest<PagedResult<ItemListResponse>>
    {
    }

    public class ListItemsQueryHandler(IItemQueries itemQueries) : IRequestHandler<ListItemsQuery, PagedResult<ItemListResponse>>
    {
        public Task<PagedResult<ItemListResponse>> Handle(
            ListItemsQuery request,
            CancellationToken cancellationToken
        )
        {
            return itemQueries.ListAsync(request, cancellationToken);
        }
    }
}
