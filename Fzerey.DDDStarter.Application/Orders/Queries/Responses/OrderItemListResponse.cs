namespace Fzerey.DDDStarter.Application.Orders.Queries.Responses
{
    public class OrderItemListResponse
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
