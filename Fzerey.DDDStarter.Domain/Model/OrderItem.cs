using Fzerey.DDDStarter.Domain.Exceptions;

namespace Fzerey.DDDStarter.Domain.Model
{
    public class OrderItem : Entity
    {
        public int OrderId { get; private set; }
        public int ItemId { get; private set; }
        public int Quantity { get; private set; }
        public Item Item { get; private set; } = null!;
        public Order Order { get; private set; } = null!;

        internal OrderItem(Item item, int quantity)
        {
            EnsurePositive(quantity);
            Item = item;
            ItemId = item.Id;
            Quantity = quantity;
        }

        private OrderItem() { }

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
