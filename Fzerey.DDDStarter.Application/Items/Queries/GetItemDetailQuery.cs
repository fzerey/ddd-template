using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;
using System;

namespace Fzerey.DDDStarter.Application.Items.Queries
{
    public class GetItemDetailQuery : IRequest<ItemDetailResponse>
    {
        public int Id { get; set; }
    }

    public class GetItemDetailQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<GetItemDetailQuery, ItemDetailResponse>
    {
        public async Task<ItemDetailResponse> Handle(GetItemDetailQuery request, CancellationToken cancellationToken)
        {
            var item = await dbContext.Items.FindAsync(request.Id) ?? throw new OrderNotFoundException();
            return new ItemDetailResponse
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            };
        }
    }

}
