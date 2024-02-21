using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Domain.Repositories;
using MediatR;

namespace Fzerey.DDDStarter.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public string? CustomerName { get; set; }
    }

    public class CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.CustomerName!);
            orderRepository.Add(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
    }
}
