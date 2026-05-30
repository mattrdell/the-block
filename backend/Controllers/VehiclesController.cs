using Microsoft.AspNetCore.Mvc;
using TheBlock.Api.Models;
using TheBlock.Api.Services;

namespace TheBlock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(IVehiclesService service) : ControllerBase
{
    private readonly IVehiclesService _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Vehicle>>> Browse(
        [FromQuery] string? search,
        [FromQuery] string? make,
        [FromQuery] string? province,
        [FromQuery] string? sort)
    {
        var vehicles = await _service.BrowseAsync(search, make, province, sort);
        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetById(string id)
    {
        var vehicle = await _service.GetByIdAsync(id);
        return vehicle is null ? NotFound() : Ok(vehicle);
    }

    [HttpPost("{id}/bids")]
    public async Task<ActionResult<Vehicle>> PlaceBid(string id, [FromBody] PlaceBidRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _service.PlaceBidAsync(id, request);
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return Ok(result.Vehicle);
    }
}

