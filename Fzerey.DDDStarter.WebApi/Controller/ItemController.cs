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
        public async Task<IActionResult> Get([FromQuery] ListItemsRequestModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await applicationService.SendRequest(new ListItemsQuery
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                SearchQuery = model.SearchQuery,
                SortBy = model.SortBy,
                SortOrder = model.SortOrder
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await applicationService.SendRequest(new GetItemDetailQuery { Id = id });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateItemRequestModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await applicationService.SendRequest(new CreateItemCommand
            {
                Name = model.Name,
                Price = model.Price
            });
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateItemRequestModel model, int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await applicationService.SendRequest(new UpdateItemCommand
            {
                Id = id,
                Name = model.Name,
                Price = model.Price
            });
            return Ok();
        }
    }
}
