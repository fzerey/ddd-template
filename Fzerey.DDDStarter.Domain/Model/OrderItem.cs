using Fzerey.DDDStarter.Domain.Exceptions;

namespace Fzerey.DDDStarter.Domain.Model
{
    public class OrderItem : Entity
    {
        public int OrderId { get; private set; }
        public int ItemId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal LineTotal => UnitPrice * Quantity;
        public Item Item { get; private set; } = null!;
        public Order Order { get; private set; } = null!;

        internal OrderItem(Item item, int quantity)
        {
            EnsurePositive(quantity);
            Item = item;
            ItemId = item.Id;
            UnitPrice = item.Price;
            Quantity = quantity;
        }

        private OrderItem() { }

        internal bool IsFor(Item item)
        {
            var sameItem = ReferenceEquals(Item, item) || (item.Id != 0 && ItemId == item.Id);
            return sameItem && UnitPrice == item.Price;
        }

        internal void IncreaseQuantity(int quantity)
        {
            EnsurePositive(quantity);
            Quantity += quantity;
        }

        private static void EnsurePositive(int quantity)
        {
            if (quantity <= 0)
            {
                throw new DomainException("Quantity must be greater than zero", DomainErrorCodes.INVALID_QUANTITY);
            }
        }
    }
}
