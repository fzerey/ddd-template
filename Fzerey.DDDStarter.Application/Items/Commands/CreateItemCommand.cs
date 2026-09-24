using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Domain.Repositories;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Commands
{
    public class CreateItemCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateItemCommandHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateItemCommand, int>
    {
        public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item(request.Name!, request.Price);
            itemRepository.Add(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return item.Id;
        }
    }
}
