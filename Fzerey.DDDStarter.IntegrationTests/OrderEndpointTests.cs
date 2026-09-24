using System.Net;
using System.Net.Http.Json;
using Fzerey.DDDStarter.Application.Common.Pagination;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;

namespace Fzerey.DDDStarter.IntegrationTests
{
    public class OrderEndpointTests(ApiFactory factory)
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Adding_the_same_item_twice_merges_quantity_and_totals()
        {
            var pen = await _client.CreateItemAsync("Pen", 3m);
            var book = await _client.CreateItemAsync("Book", 20m);
            var orderId = await _client.CreateOrderAsync("Alice");

            Assert.Equal(HttpStatusCode.NoContent, (await _client.AddItemToOrderAsync(orderId, pen, 4)).StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, (await _client.AddItemToOrderAsync(orderId, pen, 1)).StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, (await _client.AddItemToOrderAsync(orderId, book, 1)).StatusCode);

            var order = await _client.GetOrderAsync(orderId);
            Assert.Equal("Alice", order.CustomerName);
            Assert.Equal(35m, order.TotalAmount);
            Assert.Equal(2, order.OrderItems.Count);
            Assert.Equal(5, order.OrderItems.Single(i => i.Name == "Pen").Quantity);
        }

        [Fact]
        public async Task Order_keeps_unit_price_after_item_price_changes()
        {
            var pen = await _client.CreateItemAsync("Pen", 3m);
            var orderId = await _client.CreateOrderAsync("Bob");
            await _client.AddItemToOrderAsync(orderId, pen, 5);

            await _client.UpdateItemAsync(pen, "Pen", 100m);
            Assert.Equal(15m, (await _client.GetOrderAsync(orderId)).TotalAmount);

            await _client.AddItemToOrderAsync(orderId, pen, 1);
            var order = await _client.GetOrderAsync(orderId);
            Assert.Equal(115m, order.TotalAmount);
            Assert.Equal(2, order.OrderItems.Count);
        }

        [Fact]
        public async Task List_includes_order_with_its_total()
        {
            var pen = await _client.CreateItemAsync("Pen", 2m);
            var orderId = await _client.CreateOrderAsync("Carol");
            await _client.AddItemToOrderAsync(orderId, pen, 3);

            var page = await _client.GetFromJsonAsync<PagedResult<OrderListResponse>>("/api/order?pageIndex=1&pageSize=100", TestContext.Current.CancellationToken);

            var listed = Assert.Single(page!.Items, o => o.Id == orderId);
            Assert.Equal(6m, listed.TotalAmount);
        }

        [Fact]
        public async Task Get_missing_order_returns_404_with_error_code()
        {
            var response = await _client.GetAsync("/api/order/999999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
            Assert.Equal("101", error!.ErrorCode);
        }

        [Fact]
        public async Task Adding_missing_item_returns_404()
        {
            var orderId = await _client.CreateOrderAsync("Dave");

            var response = await _client.AddItemToOrderAsync(orderId, 999999, 1);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
            Assert.Equal("201", error!.ErrorCode);
        }

        [Fact]
        public async Task Adding_zero_quantity_returns_400_and_changes_nothing()
        {
            var pen = await _client.CreateItemAsync("Pen", 3m);
            var orderId = await _client.CreateOrderAsync("Eve");

            var response = await _client.AddItemToOrderAsync(orderId, pen, 0);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Empty((await _client.GetOrderAsync(orderId)).OrderItems);
        }
    }
}
