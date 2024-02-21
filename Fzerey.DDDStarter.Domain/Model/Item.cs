using Fzerey.DDDStarter.Domain.Exceptions;

namespace Fzerey.DDDStarter.Domain.Model
{
    public class Item : Entity
    {
        public const int NameMaxLength = 100;

        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }

        public Item(string name, decimal price)
        {
            Update(name, price);
        }

        private Item() { }

        public void Update(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > NameMaxLength)
            {
                throw new DomainException($"Item name must be 1-{NameMaxLength} characters", DomainErrorCodes.INVALID_ITEM_NAME);
            }
            if (price < 0)
            {
                throw new DomainException("Item price cannot be negative", DomainErrorCodes.INVALID_PRICE);
            }
            Name = name;
            Price = price;
        }
    }
}
