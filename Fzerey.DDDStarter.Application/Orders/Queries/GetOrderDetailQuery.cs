using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Queries
{
    public class GetOrderDetailQuery : IRequest<OrderDetailResponse>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderDetailQueryHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<GetOrderDetailQuery, OrderDetailResponse>
    {
        public async Task<OrderDetailResponse> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var order = await applicationDbContext.Orders.FindAsync(request.OrderId) ?? throw new OrderNotFoundException();
            return new OrderDetailResponse
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(o => new OrderItemListResponse
                {
                    Name = o.Item.Name,
                    Price = o.Item.Price,
                    Quantity = o.Quantity
                }).ToList()
            };
        }
    }
}
