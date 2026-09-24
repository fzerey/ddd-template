using System.ComponentModel.DataAnnotations;

namespace Fzerey.DDDStarter.WebApi.Models.Orders
{
    public class AddItemToOrderRequestModel
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
