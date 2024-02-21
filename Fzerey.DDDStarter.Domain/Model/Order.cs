namespace Fzerey.DDDStarter.Domain.Model
{
    public class Order : Entity
    {
        public string? CustomerName { get; set; }
        public decimal TotalAmount => OrderItems.Sum(i => i.Item.Price * i.Quantity);
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public Order(string customerName)
            : base()
        {
            CustomerName = customerName;
        }

        public Order() { 
        }

        public void AddItem(Item item, int quantity)
        {
            OrderItems ??= [];
            OrderItems.Add(new OrderItem { Item = item, Quantity = quantity });
        }
    }
}
