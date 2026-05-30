using System.Text.Json;
using System.Text.Json.Serialization;
using TheBlock.Api.Models;

namespace TheBlock.Api.Data;

public static class VehicleDataSeeder
{
    public static async Task SeedAsync(VehiclesContext context, IWebHostEnvironment environment)
    {
        if (context.Vehicles.Any())
        {
            return;
        }

        var dataPath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "data", "vehicles.json"));
        if (!File.Exists(dataPath))
        {
            throw new FileNotFoundException($"Vehicle dataset not found at {dataPath}");
        }

        var json = await File.ReadAllTextAsync(dataPath);
        var sourceVehicles = JsonSerializer.Deserialize<List<VehicleSeedRecord>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (sourceVehicles is null || sourceVehicles.Count == 0)
        {
            throw new InvalidOperationException("No vehicles were loaded from the dataset.");
        }

        var vehicles = sourceVehicles.Select(source => new Vehicle
        {
            Id = source.Id,
            Vin = source.Vin,
            Year = source.Year,
            Make = source.Make,
            Model = source.Model,
            Trim = source.Trim,
            BodyStyle = source.BodyStyle,
            ExteriorColor = source.ExteriorColor,
            InteriorColor = source.InteriorColor,
            Engine = source.Engine,
            Transmission = source.Transmission,
            Drivetrain = source.Drivetrain,
            OdometerKm = source.OdometerKm,
            FuelType = source.FuelType,
            ConditionGrade = source.ConditionGrade,
            ConditionReport = source.ConditionReport,
            DamageNotes = source.DamageNotes ?? [],
            TitleStatus = source.TitleStatus,
            Province = source.Province,
            City = source.City,
            AuctionStart = source.AuctionStart,
            StartingBid = source.StartingBid,
            ReservePrice = source.ReservePrice,
            BuyNowPrice = source.BuyNowPrice,
            Images = source.Images ?? [],
            SellingDealership = source.SellingDealership,
            Lot = source.Lot,
            CurrentBid = source.CurrentBid ?? source.StartingBid,
            BidCount = source.BidCount
        }).ToList();

        context.Vehicles.AddRange(vehicles);
        await context.SaveChangesAsync();
    }

    private sealed class VehicleSeedRecord
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("vin")]
        public string Vin { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("trim")]
        public string Trim { get; set; } = string.Empty;

        [JsonPropertyName("body_style")]
        public string BodyStyle { get; set; } = string.Empty;

        [JsonPropertyName("exterior_color")]
        public string ExteriorColor { get; set; } = string.Empty;

        [JsonPropertyName("interior_color")]
        public string InteriorColor { get; set; } = string.Empty;

        [JsonPropertyName("engine")]
        public string Engine { get; set; } = string.Empty;

        [JsonPropertyName("transmission")]
        public string Transmission { get; set; } = string.Empty;

        [JsonPropertyName("drivetrain")]
        public string Drivetrain { get; set; } = string.Empty;

        [JsonPropertyName("odometer_km")]
        public int OdometerKm { get; set; }

        [JsonPropertyName("fuel_type")]
        public string FuelType { get; set; } = string.Empty;

        [JsonPropertyName("condition_grade")]
        public decimal ConditionGrade { get; set; }

        [JsonPropertyName("condition_report")]
        public string ConditionReport { get; set; } = string.Empty;

        [JsonPropertyName("damage_notes")]
        public List<string>? DamageNotes { get; set; }

        [JsonPropertyName("title_status")]
        public string TitleStatus { get; set; } = string.Empty;

        [JsonPropertyName("province")]
        public string Province { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("auction_start")]
        public DateTime AuctionStart { get; set; }

        [JsonPropertyName("starting_bid")]
        public decimal StartingBid { get; set; }

        [JsonPropertyName("reserve_price")]
        public decimal? ReservePrice { get; set; }

        [JsonPropertyName("buy_now_price")]
        public decimal? BuyNowPrice { get; set; }

        [JsonPropertyName("images")]
        public List<string>? Images { get; set; }

        [JsonPropertyName("selling_dealership")]
        public string SellingDealership { get; set; } = string.Empty;

        [JsonPropertyName("lot")]
        public string Lot { get; set; } = string.Empty;

        [JsonPropertyName("current_bid")]
        public decimal? CurrentBid { get; set; }

        [JsonPropertyName("bid_count")]
        public int BidCount { get; set; }
    }
}
