namespace Fzerey.DDDStarter.Application.Items.Queries.Responses
{
    public class ItemListResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}
