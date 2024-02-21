using Fzerey.DDDStarter.Application;
using Fzerey.DDDStarter.Application.Orders.Commands;
using Fzerey.DDDStarter.Application.Orders.Queries;
using Fzerey.DDDStarter.WebApi.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Fzerey.DDDStarter.WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IApplicationService applicationService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListOrdersRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var request = new ListOrdersQuery
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                SearchQuery = model.SearchQuery,
                SortBy = model.SortBy,
                SortOrder = model.SortOrder
            };
            var response = await applicationService.SendRequest(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new GetOrderDetailQuery { OrderId = id };
            var response = await applicationService.SendRequest(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var request = new CreateOrderCommand { CustomerName = model.CustomerName, };
            await applicationService.SendRequest(request);
            return Ok();
        }

        [HttpPost("{id}/items/{itemId}")]
        public async Task<IActionResult> AddItem(int id, int itemId, [FromBody] AddItemToOrderRequestModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var request = new AddItemToOrderCommand { OrderId = id, ItemId = itemId, Quantity = model.Quantity };
            await applicationService.SendRequest(request);
            return Ok();
        }
    }
}
