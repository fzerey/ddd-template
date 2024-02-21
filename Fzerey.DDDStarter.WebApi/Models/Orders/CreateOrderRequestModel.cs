using System.ComponentModel.DataAnnotations;

namespace Fzerey.DDDStarter.WebApi.Models.Orders
{
    public class CreateOrderRequestModel
    {
        [Required]
        public string? CustomerName { get; set; }
    }
}
