using Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems;
using Fzerey.DDDStarter.Application.Items.Commands;
using Fzerey.DDDStarter.Domain.Exceptions;
using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Tests.Fakes;

namespace Fzerey.DDDStarter.Tests.Application
{
    public class ItemHandlerTests
    {
        private readonly InMemoryItemRepository _items = new();
        private readonly FakeUnitOfWork _unitOfWork = new();

        [Fact]
        public async Task CreateItem_adds_item_and_saves()
        {
            var handler = new CreateItemCommandHandler(_items, _unitOfWork);

            await handler.Handle(new CreateItemCommand { Name = "Pen", Price = 10m }, TestContext.Current.CancellationToken);

            var item = Assert.Single(_items.Items);
            Assert.Equal("Pen", item.Name);
            Assert.Equal(1, _unitOfWork.SaveCount);
        }

        [Fact]
        public async Task CreateItem_with_invalid_price_does_not_save()
        {
            var handler = new CreateItemCommandHandler(_items, _unitOfWork);

            await Assert.ThrowsAsync<DomainException>(() =>
                handler.Handle(new CreateItemCommand { Name = "Pen", Price = -1m }, TestContext.Current.CancellationToken));
            Assert.Empty(_items.Items);
            Assert.Equal(0, _unitOfWork.SaveCount);
        }

        [Fact]
        public async Task UpdateItem_updates_and_saves()
        {
            var item = new Item("Pen", 10m).WithId(3);
            _items.Add(item);
            var handler = new UpdateItemCommandHandler(_items, _unitOfWork);

            await handler.Handle(new UpdateItemCommand { Id = 3, Name = "Pencil", Price = 12m }, TestContext.Current.CancellationToken);

            Assert.Equal("Pencil", item.Name);
            Assert.Equal(12m, item.Price);
            Assert.Equal(1, _unitOfWork.SaveCount);
        }

        [Fact]
        public async Task UpdateItem_throws_when_item_missing()
        {
            var handler = new UpdateItemCommandHandler(_items, _unitOfWork);

            await Assert.ThrowsAsync<ItemNotFoundException>(() =>
                handler.Handle(new UpdateItemCommand { Id = 99, Name = "Pencil", Price = 12m }, TestContext.Current.CancellationToken));
        }
    }
}
