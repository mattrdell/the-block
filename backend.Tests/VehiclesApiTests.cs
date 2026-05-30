using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TheBlock.Api.Tests;

public class VehiclesApiTests
{
    [Fact]
    public async Task GetVehicles_ReturnsSeededInventory()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/vehicles");

        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(payload);

        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.Equal(200, document.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task PlaceBid_UpdatesBidAndReturnsVehicle()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var listResponse = await client.GetAsync("/api/vehicles");
        listResponse.EnsureSuccessStatusCode();

        var listPayload = await listResponse.Content.ReadAsStringAsync();
        using var listDocument = JsonDocument.Parse(listPayload);
        var firstVehicle = listDocument.RootElement[0];

        var id = firstVehicle.GetProperty("id").GetString();
        var currentBid = firstVehicle.GetProperty("current_bid").GetDecimal();
        var startingBid = firstVehicle.GetProperty("starting_bid").GetDecimal();
        var originalBidCount = firstVehicle.GetProperty("bid_count").GetInt32();

        Assert.False(string.IsNullOrWhiteSpace(id));

        var minimumBid = Math.Max(currentBid + 100m, startingBid);

        var postResponse = await client.PostAsJsonAsync($"/api/vehicles/{id}/bids", new { amount = minimumBid });
        postResponse.EnsureSuccessStatusCode();

        var updatedPayload = await postResponse.Content.ReadAsStringAsync();
        using var updatedDocument = JsonDocument.Parse(updatedPayload);

        Assert.Equal(minimumBid, updatedDocument.RootElement.GetProperty("current_bid").GetDecimal());
        Assert.Equal(originalBidCount + 1, updatedDocument.RootElement.GetProperty("bid_count").GetInt32());

        var detailResponse = await client.GetAsync($"/api/vehicles/{id}");
        detailResponse.EnsureSuccessStatusCode();

        var detailPayload = await detailResponse.Content.ReadAsStringAsync();
        using var detailDocument = JsonDocument.Parse(detailPayload);

        Assert.Equal(minimumBid, detailDocument.RootElement.GetProperty("current_bid").GetDecimal());
        Assert.Equal(originalBidCount + 1, detailDocument.RootElement.GetProperty("bid_count").GetInt32());
    }

    [Fact]
    public async Task PlaceBid_ReturnsBadRequest_WhenBidIsTooLow()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var listResponse = await client.GetAsync("/api/vehicles");
        listResponse.EnsureSuccessStatusCode();

        var listPayload = await listResponse.Content.ReadAsStringAsync();
        using var listDocument = JsonDocument.Parse(listPayload);
        var firstVehicle = listDocument.RootElement[0];

        var id = firstVehicle.GetProperty("id").GetString();
        var currentBid = firstVehicle.GetProperty("current_bid").GetDecimal();
        var startingBid = firstVehicle.GetProperty("starting_bid").GetDecimal();
        var minimumBid = Math.Max(currentBid + 100m, startingBid);
        var invalidBid = minimumBid - 1m;

        var response = await client.PostAsJsonAsync($"/api/vehicles/{id}/bids", new { amount = invalidBid });

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var payload = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(payload);
        var message = document.RootElement.GetProperty("message").GetString();
        Assert.Contains("at least", message, StringComparison.OrdinalIgnoreCase);
    }
}
