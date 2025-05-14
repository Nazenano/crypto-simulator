using CryptoSimulator.DataContext.Dtos;
using Microsoft.AspNetCore.Authorization;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[ApiController]
[Route("api/crypto/price")]
public class PriceUpdateController : ControllerBase
{
    private readonly IPriceUpdateService _priceUpdateService;

    public PriceUpdateController(IPriceUpdateService priceUpdateService)
    {
        _priceUpdateService = priceUpdateService;
    }

    [HttpPut]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdatePrice([FromBody] PriceUpdateDto dto)
    {
        try
        {
            await _priceUpdateService.UpdatePrice(dto);
            return Ok("Price updated successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("history/{cryptoId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<IEnumerable<PriceHistoryDto>>> GetPriceHistory(int cryptoId)
    {
        try
        {
            var history = await _priceUpdateService.GetPriceHistory(cryptoId);
            return Ok(history);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}
