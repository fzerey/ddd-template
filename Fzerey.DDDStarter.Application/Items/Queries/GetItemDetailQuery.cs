using Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems;
using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Queries
{
    public class GetItemDetailQuery : IRequest<ItemDetailResponse>
    {
        public int Id { get; set; }
    }

    public class GetItemDetailQueryHandler(IItemQueries itemQueries) : IRequestHandler<GetItemDetailQuery, ItemDetailResponse>
    {
        public async Task<ItemDetailResponse> Handle(GetItemDetailQuery request, CancellationToken cancellationToken)
        {
            return await itemQueries.GetDetailAsync(request.Id, cancellationToken)
                ?? throw new ItemNotFoundException();
        }
    }
}
