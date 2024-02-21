using Fzerey.DDDStarter.Application.Items.Queries.Responses;
namespace Fzerey.DDDStarter.Application.Orders.Queries.Responses
{
    public class OrderDetailResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemListResponse> OrderItems { get; set; }
    }
}
