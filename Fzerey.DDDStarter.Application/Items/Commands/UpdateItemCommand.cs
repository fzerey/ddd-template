using Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems;
using Fzerey.DDDStarter.Domain.Repositories;
using MediatR;

namespace Fzerey.DDDStarter.Application.Items.Commands
{
    public class UpdateItemCommand : IRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateItemCommandHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateItemCommand>
    {
        public async Task Handle(
            UpdateItemCommand request,
            CancellationToken cancellationToken
        )
        {
            var item =
                await itemRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new ItemNotFoundException();
            item.Update(request.Name!, request.Price);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
