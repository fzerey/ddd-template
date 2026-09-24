namespace Fzerey.DDDStarter.Application.Orders.Queries.Responses
{
    public class OrderListResponse
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
