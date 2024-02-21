using Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems;
using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Commands
{
    public class AddItemToOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddItemToOrderCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<AddItemToOrderCommand>
    {
        public async Task Handle(AddItemToOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await dbContext.Orders.FindAsync(request.OrderId) ?? throw new OrderNotFoundException();
            var item = await dbContext.Items.FindAsync(request.ItemId) ?? throw new ItemNotFoundException();
            order.AddItem(item, request.Quantity);
            dbContext.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

