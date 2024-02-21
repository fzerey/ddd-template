using Fzerey.DDDStarter.Application.Common.Exceptions.OrderItems;
using Fzerey.DDDStarter.Application.Common.Exceptions.Orders;
using Fzerey.DDDStarter.Application.Orders.Commands;
using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Tests.Fakes;

namespace Fzerey.DDDStarter.Tests.Application
{
    public class OrderHandlerTests
    {
        private readonly InMemoryOrderRepository _orders = new();
        private readonly InMemoryItemRepository _items = new();
        private readonly FakeUnitOfWork _unitOfWork = new();

        [Fact]
        public async Task CreateOrder_adds_order_and_saves()
        {
            var handler = new CreateOrderCommandHandler(_orders, _unitOfWork);

            await handler.Handle(new CreateOrderCommand { CustomerName = "Alice" }, TestContext.Current.CancellationToken);

            Assert.Equal("Alice", Assert.Single(_orders.Orders).CustomerName);
            Assert.Equal(1, _unitOfWork.SaveCount);
        }

        [Fact]
        public async Task AddItemToOrder_adds_line_and_saves()
        {
            var order = new Order("Alice").WithId(1);
            _orders.Add(order);
            _items.Add(new Item("Pen", 10m).WithId(7));
            var handler = new AddItemToOrderCommandHandler(_orders, _items, _unitOfWork);

            await handler.Handle(new AddItemToOrderCommand { OrderId = 1, ItemId = 7, Quantity = 2 }, TestContext.Current.CancellationToken);

            var line = Assert.Single(order.OrderItems);
            Assert.Equal(7, line.ItemId);
            Assert.Equal(2, line.Quantity);
            Assert.Equal(1, _unitOfWork.SaveCount);
        }

        [Fact]
        public async Task AddItemToOrder_throws_when_order_missing()
        {
            _items.Add(new Item("Pen", 10m).WithId(7));
            var handler = new AddItemToOrderCommandHandler(_orders, _items, _unitOfWork);

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                handler.Handle(new AddItemToOrderCommand { OrderId = 99, ItemId = 7, Quantity = 1 }, TestContext.Current.CancellationToken));
            Assert.Equal(0, _unitOfWork.SaveCount);
        }

        [Fact]
        public async Task AddItemToOrder_throws_when_item_missing()
        {
            _orders.Add(new Order("Alice").WithId(1));
            var handler = new AddItemToOrderCommandHandler(_orders, _items, _unitOfWork);

            await Assert.ThrowsAsync<ItemNotFoundException>(() =>
                handler.Handle(new AddItemToOrderCommand { OrderId = 1, ItemId = 99, Quantity = 1 }, TestContext.Current.CancellationToken));
            Assert.Equal(0, _unitOfWork.SaveCount);
        }
    }
}
