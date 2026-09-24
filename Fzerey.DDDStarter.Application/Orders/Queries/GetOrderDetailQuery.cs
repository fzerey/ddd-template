using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Queries
{
    public class GetOrderDetailQuery : IRequest<OrderDetailResponse>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderDetailQueryHandler(IOrderQueries orderQueries) : IRequestHandler<GetOrderDetailQuery, OrderDetailResponse>
    {
        public async Task<OrderDetailResponse> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            return await orderQueries.GetDetailAsync(request.OrderId, cancellationToken)
                ?? throw new OrderNotFoundException();
        }
    }
}
