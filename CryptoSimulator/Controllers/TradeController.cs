using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[Route("api/trade")]
[ApiController]
public class TradeController : ControllerBase
{
    private readonly ITradeService _tradeService;

    public TradeController(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpPost("buy")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> Buy([FromQuery] int userId, [FromBody] TradeCreateDto dto)
    {
        try
        {
            TradeDto transaction = await _tradeService.Buy(userId, dto);
            return CreatedAtAction(
                actionName: nameof(TransactionController.Get),
                controllerName: "Transaction",
                routeValues: new { userId },
                value: transaction);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost("sell")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> Sell([FromQuery] int userId, [FromBody] TradeCreateDto dto)
    {
        try
        {
            TradeDto transaction = await _tradeService.Sell(userId, dto);
            return CreatedAtAction(
                actionName: nameof(TransactionController.Get),
                controllerName: "Transaction",
                routeValues: new { userId },
                value: transaction);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}
