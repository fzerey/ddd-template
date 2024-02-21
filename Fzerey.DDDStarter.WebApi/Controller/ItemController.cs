using Fzerey.DDDStarter.Application;
using Fzerey.DDDStarter.Application.Items.Commands;
using Fzerey.DDDStarter.Application.Items.Queries;
using Fzerey.DDDStarter.WebApi.Models.Items;
using Microsoft.AspNetCore.Mvc;

namespace Fzerey.DDDStarter.WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController(IApplicationService applicationService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListItemsRequestModel model, CancellationToken cancellationToken)
        {
            var response = await applicationService.SendRequest(new ListItemsQuery
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                SearchQuery = model.SearchQuery,
                SortBy = model.SortBy,
                SortOrder = model.SortOrder
            }, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var response = await applicationService.SendRequest(new GetItemDetailQuery { Id = id }, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateItemRequestModel model, CancellationToken cancellationToken)
        {
            var id = await applicationService.SendRequest(new CreateItemCommand
            {
                Name = model.Name,
                Price = model.Price
            }, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromBody] UpdateItemRequestModel model, int id, CancellationToken cancellationToken)
        {
            await applicationService.SendRequest(new UpdateItemCommand
            {
                Id = id,
                Name = model.Name,
                Price = model.Price
            }, cancellationToken);
            return NoContent();
        }
    }
}
