using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Commands
{
    public class UpdateItemCommand : IRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateItemCommandHandler(ApplicationDbContext dbContext)
        : IRequestHandler<UpdateItemCommand>
    {
        public async Task Handle(
            UpdateItemCommand request,
            CancellationToken cancellationToken
        )
        {
            var item =
                await dbContext.Items.FindAsync(request.Id)
                ?? throw new OrderNotFoundException();
            item.Name = request.Name!;
            item.Price = request.Price;
            dbContext.Update(item);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
