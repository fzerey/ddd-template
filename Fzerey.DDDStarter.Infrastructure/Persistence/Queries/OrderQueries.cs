using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;
using Fzerey.DDDStarter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Queries
{
    public class OrderQueries(ApplicationDbContext dbContext) : IOrderQueries
    {
        public Task<OrderDetailResponse?> GetDetailAsync(int id, CancellationToken cancellationToken = default)
        {
            return dbContext.Orders
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailResponse
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice),
                    OrderItems = o.OrderItems.Select(oi => new OrderItemListResponse
                    {
                        Name = oi.Item.Name,
                        Price = oi.UnitPrice,
                        Quantity = oi.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<PagedResult<OrderListResponse>> ListAsync(PageRequest request, CancellationToken cancellationToken = default)
        {
            return dbContext.Orders
                .AsNoTracking()
                .OrderBy(o => o.Id)
                .Select(o => new OrderListResponse
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .ToPagedResultAsync(request, cancellationToken);
        }
    }
}
