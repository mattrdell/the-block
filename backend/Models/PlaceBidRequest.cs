using System.ComponentModel.DataAnnotations;

namespace TheBlock.Api.Models;

public class PlaceBidRequest
{
    [Range(1, 2000000)]
    public decimal Amount { get; set; }

    [MaxLength(80)]
    public string? BidderName { get; set; }
}

