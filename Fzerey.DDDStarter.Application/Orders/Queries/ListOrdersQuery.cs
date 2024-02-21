using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;
using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Queries
{
    public class ListOrdersQuery : PageRequest, IRequest<PagedResult<OrderListResponse>> { }

    public class ListOrdersQueryHandler(ApplicationDbContext dbContext)
        : IRequestHandler<ListOrdersQuery, PagedResult<OrderListResponse>>
    {
        public async Task<PagedResult<OrderListResponse>> Handle(
            ListOrdersQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = new PagedResult<OrderListResponse>();
            var orders = dbContext
                .Orders.AsQueryable()
                .Select(o => new OrderListResponse
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName
                });
            await result.ToPagedList(orders, request.PageIndex, request.PageSize);

            result.Items.ForEach(o => o.TotalAmount = dbContext.OrderItems
                .Where(oi => oi.OrderId == o.Id)
                .Sum(oi => oi.Quantity * oi.Item.Price));
            return result;
        }
    }
}
