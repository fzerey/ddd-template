using System.Net;
using System.Net.Http.Json;
using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fzerey.DDDStarter.IntegrationTests
{
    public class ItemEndpointTests(ApiFactory factory)
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Create_returns_201_with_location_of_the_new_item()
        {
            var response = await _client.PostAsJsonAsync("/api/item", new { name = "Pen", price = 2.5m }, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<CreatedResponse>(TestContext.Current.CancellationToken);
            Assert.EndsWith($"/api/Item/{created!.Id}", response.Headers.Location!.ToString());

            var item = await _client.GetFromJsonAsync<ItemDetailResponse>(response.Headers.Location, TestContext.Current.CancellationToken);
            Assert.Equal("Pen", item!.Name);
            Assert.Equal(2.5m, item.Price);
        }

        [Fact]
        public async Task Update_returns_204_and_persists_changes()
        {
            var id = await _client.CreateItemAsync("Pen", 2.5m);

            var response = await _client.UpdateItemAsync(id, "Pencil", 3m);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var item = await _client.GetItemAsync(id);
            Assert.Equal("Pencil", item.Name);
            Assert.Equal(3m, item.Price);
        }

        [Fact]
        public async Task Get_missing_item_returns_404_with_error_code()
        {
            var response = await _client.GetAsync("/api/item/999999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
            Assert.Equal("201", error!.ErrorCode);
        }

        [Fact]
        public async Task Create_with_invalid_body_returns_400_with_field_errors()
        {
            var response = await _client.PostAsJsonAsync("/api/item", new { name = "", price = -1m }, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);
            Assert.Contains("Name", problem!.Errors.Keys);
            Assert.Contains("Price", problem.Errors.Keys);
        }

        [Fact]
        public async Task List_returns_requested_page_size()
        {
            await _client.CreateItemAsync("A", 1m);
            await _client.CreateItemAsync("B", 1m);

            var page = await _client.GetFromJsonAsync<PagedResult<ItemListResponse>>("/api/item?pageIndex=1&pageSize=1", TestContext.Current.CancellationToken);

            Assert.Single(page!.Items);
            Assert.Equal(1, page.PageSize);
            Assert.True(page.TotalCount >= 2);
            Assert.Equal(page.TotalCount, page.TotalPages);
        }

        [Theory]
        [InlineData("pageIndex=0")]
        [InlineData("pageSize=0")]
        [InlineData("pageSize=101")]
        public async Task List_with_invalid_paging_returns_400(string query)
        {
            var response = await _client.GetAsync($"/api/item?{query}", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
