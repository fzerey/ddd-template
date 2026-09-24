using Fzerey.DDDStarter.Domain.Exceptions;
using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Tests.Fakes;

namespace Fzerey.DDDStarter.Tests.Domain
{
    public class OrderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_rejects_blank_customer_name(string customerName)
        {
            var ex = Assert.Throws<DomainException>(() => new Order(customerName));
            Assert.Equal(DomainErrorCodes.INVALID_CUSTOMER_NAME, ex.Code);
        }

        [Fact]
        public void Constructor_rejects_too_long_customer_name()
        {
            var name = new string('a', Order.CustomerNameMaxLength + 1);
            Assert.Throws<DomainException>(() => new Order(name));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddItem_rejects_non_positive_quantity(int quantity)
        {
            var order = new Order("Alice");
            var item = new Item("Pen", 10m).WithId(1);

            var ex = Assert.Throws<DomainException>(() => order.AddItem(item, quantity));
            Assert.Equal(DomainErrorCodes.INVALID_QUANTITY, ex.Code);
            Assert.Empty(order.OrderItems);
        }

        [Fact]
        public void AddItem_merges_quantity_for_same_item()
        {
            var order = new Order("Alice");
            var item = new Item("Pen", 10m).WithId(1);

            order.AddItem(item, 2);
            order.AddItem(item, 3);

            var line = Assert.Single(order.OrderItems);
            Assert.Equal(5, line.Quantity);
        }

        [Fact]
        public void AddItem_merges_same_item_loaded_as_different_instance()
        {
            var order = new Order("Alice");

            order.AddItem(new Item("Pen", 10m).WithId(1), 2);
            order.AddItem(new Item("Pen", 10m).WithId(1), 1);

            Assert.Equal(3, Assert.Single(order.OrderItems).Quantity);
        }

        [Fact]
        public void AddItem_keeps_unsaved_items_as_separate_lines()
        {
            var order = new Order("Alice");

            order.AddItem(new Item("Pen", 10m), 1);
            order.AddItem(new Item("Book", 20m), 1);

            Assert.Equal(2, order.OrderItems.Count);
        }

        [Fact]
        public void TotalAmount_sums_price_times_quantity()
        {
            var order = new Order("Alice");

            order.AddItem(new Item("Pen", 2.5m).WithId(1), 4);
            order.AddItem(new Item("Book", 20m).WithId(2), 1);

            Assert.Equal(30m, order.TotalAmount);
        }

        [Fact]
        public void AddItem_captures_unit_price_at_order_time()
        {
            var order = new Order("Alice");
            var item = new Item("Pen", 10m).WithId(1);

            order.AddItem(item, 2);
            item.Update("Pen", 15m);

            Assert.Equal(10m, Assert.Single(order.OrderItems).UnitPrice);
            Assert.Equal(20m, order.TotalAmount);
        }

        [Fact]
        public void AddItem_after_price_change_adds_separate_line()
        {
            var order = new Order("Alice");
            var item = new Item("Pen", 10m).WithId(1);

            order.AddItem(item, 2);
            item.Update("Pen", 15m);
            order.AddItem(item, 1);

            Assert.Equal(2, order.OrderItems.Count);
            Assert.Equal(35m, order.TotalAmount);
        }
    }
}
