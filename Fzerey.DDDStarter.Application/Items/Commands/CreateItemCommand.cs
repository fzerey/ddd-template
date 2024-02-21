using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Infrastructure.Context;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Commands
{
    public class CreateItemCommand : IRequest
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateItemCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateItemCommand>
    {
        public async Task Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item
            {
                Name = request.Name!,
                Price = request.Price
            };
            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
