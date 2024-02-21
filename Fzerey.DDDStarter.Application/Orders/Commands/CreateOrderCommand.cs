using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public string? CustomerName { get; set; }
    }

    public class CreateOrderCommandHandler(ApplicationDbContext context) : IRequestHandler<CreateOrderCommand>
    {
        public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerName = request.CustomerName
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}

