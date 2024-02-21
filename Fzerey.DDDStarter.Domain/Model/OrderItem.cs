namespace Fzerey.DDDStarter.Domain.Model
{
    public class OrderItem : Entity
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
