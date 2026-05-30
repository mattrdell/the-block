using TheBlock.Api.Models;

namespace TheBlock.Api.Services;

public interface IVehiclesService
{
    Task<IReadOnlyList<Vehicle>> BrowseAsync(string? search, string? make, string? province, string? sort);
    Task<Vehicle?> GetByIdAsync(string id);
    Task<BidPlacementResult> PlaceBidAsync(string id, PlaceBidRequest request);
}

public sealed record BidPlacementResult(bool Success, string? ErrorMessage, Vehicle? Vehicle);

