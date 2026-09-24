namespace Fzerey.DDDStarter.Application.Orders.Queries.Responses
{
    public class OrderDetailResponse
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemListResponse> OrderItems { get; set; } = [];
    }
}
