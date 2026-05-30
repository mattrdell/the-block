using Microsoft.EntityFrameworkCore;
using TheBlock.Api.Data;
using TheBlock.Api.Models;

namespace TheBlock.Api.Services;

public class VehiclesService(VehiclesContext context) : IVehiclesService
{
    private readonly VehiclesContext _context = context;

    public async Task<IReadOnlyList<Vehicle>> BrowseAsync(string? search, string? make, string? province, string? sort)
    {
        var query = _context.Vehicles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim().ToLowerInvariant();
            query = query.Where(v =>
                v.Make.ToLower().Contains(keyword) ||
                v.Model.ToLower().Contains(keyword) ||
                v.Trim.ToLower().Contains(keyword) ||
                v.Vin.ToLower().Contains(keyword) ||
                v.Lot.ToLower().Contains(keyword) ||
                v.SellingDealership.ToLower().Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(make))
        {
            var normalizedMake = make.Trim().ToLowerInvariant();
            query = query.Where(v => v.Make.ToLower() == normalizedMake);
        }

        if (!string.IsNullOrWhiteSpace(province))
        {
            var normalizedProvince = province.Trim().ToLowerInvariant();
            query = query.Where(v => v.Province.ToLower() == normalizedProvince);
        }

        query = sort?.ToLowerInvariant() switch
        {
            "ending" => query.OrderBy(v => v.AuctionStart),
            "pricehigh" => query.OrderByDescending(v => v.CurrentBid),
            "pricelow" => query.OrderBy(v => v.CurrentBid),
            "grade" => query.OrderByDescending(v => v.ConditionGrade),
            _ => query.OrderByDescending(v => v.AuctionStart)
        };

        return await query.Take(200).ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(string id)
    {
        return await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<BidPlacementResult> PlaceBidAsync(string id, PlaceBidRequest request)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null)
        {
            return new BidPlacementResult(false, "Vehicle not found.", null);
        }

        var minimumNextBid = Math.Max(vehicle.CurrentBid + 100m, vehicle.StartingBid);
        if (request.Amount < minimumNextBid)
        {
            return new BidPlacementResult(false, $"Bid must be at least ${minimumNextBid:N0}.", null);
        }

        if (vehicle.BuyNowPrice is not null && request.Amount >= vehicle.BuyNowPrice.Value)
        {
            vehicle.CurrentBid = vehicle.BuyNowPrice.Value;
        }
        else
        {
            vehicle.CurrentBid = request.Amount;
        }

        vehicle.BidCount += 1;

        await _context.SaveChangesAsync();
        return new BidPlacementResult(true, null, vehicle);
    }
}

