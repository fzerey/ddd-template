using Fzerey.DDDStarter.Domain.Exceptions;

namespace Fzerey.DDDStarter.Domain.Model
{
    public class Order : Entity
    {
        public const int CustomerNameMaxLength = 64;

        private readonly List<OrderItem> _orderItems = [];

        public string CustomerName { get; private set; } = null!;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public decimal TotalAmount => _orderItems.Sum(i => i.LineTotal);

        public Order(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName) || customerName.Length > CustomerNameMaxLength)
            {
                throw new DomainException($"Customer name must be 1-{CustomerNameMaxLength} characters", DomainErrorCodes.INVALID_CUSTOMER_NAME);
            }
            CustomerName = customerName;
        }

        private Order() { }

        public void AddItem(Item item, int quantity)
        {
            ArgumentNullException.ThrowIfNull(item);

            var existing = _orderItems.FirstOrDefault(i => i.IsFor(item));
            if (existing is not null)
            {
                existing.IncreaseQuantity(quantity);
                return;
            }
            _orderItems.Add(new OrderItem(item, quantity));
        }
    }
}
