using System.ComponentModel.DataAnnotations;
using Fzerey.DDDStarter.Domain.Model;

namespace Fzerey.DDDStarter.WebApi.Models.Items
{
    public class CreateItemRequestModel
    {
        [Required]
        [StringLength(Item.NameMaxLength)]
        public string? Name { get; set; }

        [Range(0d, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
