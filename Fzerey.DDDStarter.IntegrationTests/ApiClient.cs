using System.Net;
using System.Net.Http.Json;
using Fzerey.DDDStarter.Application.Items.Queries.Responses;
using Fzerey.DDDStarter.Application.Orders.Queries.Responses;

namespace Fzerey.DDDStarter.IntegrationTests
{
    public record CreatedResponse(int Id);

    public record ErrorResponse(string? ErrorCode, string Message);

    internal static class ApiClient
    {
        public static async Task<int> CreateItemAsync(this HttpClient client, string name, decimal price)
        {
            var response = await client.PostAsJsonAsync("/api/item", new { name, price }, TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<CreatedResponse>(TestContext.Current.CancellationToken))!.Id;
        }

        public static async Task<int> CreateOrderAsync(this HttpClient client, string customerName)
        {
            var response = await client.PostAsJsonAsync("/api/order", new { customerName }, TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<CreatedResponse>(TestContext.Current.CancellationToken))!.Id;
        }

        public static Task<HttpResponseMessage> AddItemToOrderAsync(this HttpClient client, int orderId, int itemId, int quantity)
        {
            return client.PostAsJsonAsync($"/api/order/{orderId}/items/{itemId}", new { quantity }, TestContext.Current.CancellationToken);
        }

        public static Task<HttpResponseMessage> UpdateItemAsync(this HttpClient client, int itemId, string name, decimal price)
        {
            return client.PutAsJsonAsync($"/api/item/{itemId}", new { name, price }, TestContext.Current.CancellationToken);
        }

        public static async Task<OrderDetailResponse> GetOrderAsync(this HttpClient client, int orderId)
        {
            return (await client.GetFromJsonAsync<OrderDetailResponse>($"/api/order/{orderId}", TestContext.Current.CancellationToken))!;
        }

        public static async Task<ItemDetailResponse> GetItemAsync(this HttpClient client, int itemId)
        {
            return (await client.GetFromJsonAsync<ItemDetailResponse>($"/api/item/{itemId}", TestContext.Current.CancellationToken))!;
        }
    }
}
