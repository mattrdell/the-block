using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheBlock.Api.Models;

public class Vehicle
{
    [Key]
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
    public List<string> DamageNotes { get; set; } = [];

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
    public List<string> Images { get; set; } = [];

    [JsonPropertyName("selling_dealership")]
    public string SellingDealership { get; set; } = string.Empty;

    [JsonPropertyName("lot")]
    public string Lot { get; set; } = string.Empty;

    [JsonPropertyName("current_bid")]
    public decimal CurrentBid { get; set; }

    [JsonPropertyName("bid_count")]
    public int BidCount { get; set; }
}

