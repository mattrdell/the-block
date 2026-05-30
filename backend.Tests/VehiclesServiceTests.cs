using Microsoft.EntityFrameworkCore;
using TheBlock.Api.Data;
using TheBlock.Api.Models;
using TheBlock.Api.Services;

namespace TheBlock.Api.Tests;

public class VehiclesServiceTests
{
    [Fact]
    public async Task PlaceBidAsync_RejectsLowBid()
    {
        await using var context = BuildContext();
        var service = new VehiclesService(context);

        var result = await service.PlaceBidAsync("vehicle-1", new PlaceBidRequest { Amount = 10000m });

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task PlaceBidAsync_UpdatesBidAndCount()
    {
        await using var context = BuildContext();
        var service = new VehiclesService(context);

        var result = await service.PlaceBidAsync("vehicle-1", new PlaceBidRequest { Amount = 16600m });

        Assert.True(result.Success);
        Assert.NotNull(result.Vehicle);
        Assert.Equal(16600m, result.Vehicle!.CurrentBid);
        Assert.Equal(6, result.Vehicle.BidCount);
    }

    private static VehiclesContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<VehiclesContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new VehiclesContext(options);
        context.Vehicles.Add(new Vehicle
        {
            Id = "vehicle-1",
            Vin = "VIN123",
            Year = 2024,
            Make = "Honda",
            Model = "Civic",
            Trim = "Sport",
            BodyStyle = "Sedan",
            ExteriorColor = "Blue",
            InteriorColor = "Black",
            Engine = "2.0L I4",
            Transmission = "automatic",
            Drivetrain = "FWD",
            OdometerKm = 12000,
            FuelType = "gasoline",
            ConditionGrade = 4.3m,
            ConditionReport = "Solid condition",
            DamageNotes = [],
            TitleStatus = "clean",
            Province = "Ontario",
            City = "Toronto",
            AuctionStart = DateTime.UtcNow,
            StartingBid = 15000m,
            ReservePrice = null,
            BuyNowPrice = null,
            Images = ["https://placehold.co/800x600"],
            SellingDealership = "Demo Motors",
            Lot = "A-001",
            CurrentBid = 16000m,
            BidCount = 5
        });
        context.SaveChanges();

        return context;
    }
}

