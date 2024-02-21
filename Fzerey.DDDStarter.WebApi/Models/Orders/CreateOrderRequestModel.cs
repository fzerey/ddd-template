using System.ComponentModel.DataAnnotations;
using Fzerey.DDDStarter.Domain.Model;

namespace Fzerey.DDDStarter.WebApi.Models.Orders
{
    public class CreateOrderRequestModel
    {
        [Required]
        [StringLength(Order.CustomerNameMaxLength)]
        public string? CustomerName { get; set; }
    }
}
