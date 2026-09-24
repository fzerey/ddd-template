using Fzerey.DDDStarter.Domain.Exceptions;
using Fzerey.DDDStarter.Domain.Model;

namespace Fzerey.DDDStarter.Tests.Domain
{
    public class ItemTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_rejects_blank_name(string name)
        {
            var ex = Assert.Throws<DomainException>(() => new Item(name, 1m));
            Assert.Equal(DomainErrorCodes.INVALID_ITEM_NAME, ex.Code);
        }

        [Fact]
        public void Constructor_rejects_too_long_name()
        {
            Assert.Throws<DomainException>(() => new Item(new string('a', Item.NameMaxLength + 1), 1m));
        }

        [Fact]
        public void Constructor_rejects_negative_price()
        {
            var ex = Assert.Throws<DomainException>(() => new Item("Pen", -0.01m));
            Assert.Equal(DomainErrorCodes.INVALID_PRICE, ex.Code);
        }

        [Fact]
        public void Update_changes_name_and_price()
        {
            var item = new Item("Pen", 1m);

            item.Update("Pencil", 2m);

            Assert.Equal("Pencil", item.Name);
            Assert.Equal(2m, item.Price);
        }

        [Fact]
        public void Update_with_invalid_values_leaves_item_unchanged()
        {
            var item = new Item("Pen", 1m);

            Assert.Throws<DomainException>(() => item.Update("Pencil", -5m));

            Assert.Equal("Pen", item.Name);
            Assert.Equal(1m, item.Price);
        }
    }
}
