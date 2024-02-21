using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Queries
{
    public class ListItemsQuery : PageRequest, IRequest<PagedResult<ItemListResponse>>
    {
    }

    public class ListItemsQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<ListItemsQuery, PagedResult<ItemListResponse>>
    {
        public async Task<PagedResult<ItemListResponse>> Handle(
            ListItemsQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = new PagedResult<ItemListResponse>();
            var items = dbContext
                .Items.AsQueryable()
                .Select(o => new ItemListResponse
                {
                    Id = o.Id,
                    Name = o.Name,
                    Price = o.Price
                });
            await result.ToPagedList(items, request.PageIndex, request.PageSize);
            return result;
        }
    }   
}
