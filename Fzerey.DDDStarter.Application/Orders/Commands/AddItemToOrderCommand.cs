using Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems;
using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Domain.Repositories;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Commands
{
    public class AddItemToOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddItemToOrderCommandHandler(
        IOrderRepository orderRepository,
        IItemRepository itemRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<AddItemToOrderCommand>
    {
        public async Task Handle(AddItemToOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken) ?? throw new OrderNotFoundException();
            var item = await itemRepository.GetByIdAsync(request.ItemId, cancellationToken) ?? throw new ItemNotFoundException();
            order.AddItem(item, request.Quantity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
