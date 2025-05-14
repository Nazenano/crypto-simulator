using CryptoSimulator.DataContext.Dtos;
using Microsoft.AspNetCore.Authorization;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[ApiController]
[Route("api/profit")]
public class ProfitController : ControllerBase
{
    private readonly IProfitService _profitService;

    public ProfitController(IProfitService profitService)
    {
        _profitService = profitService;
    }

    [HttpGet("{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<ProfitDto>> GetTotalProfit(int userId)
    {
        try
        {
            var profit = await _profitService.GetTotalProfit(userId);
            return Ok(profit);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("details/{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<IEnumerable<ProfitDetailDto>>> GetDetailedProfit(int userId)
    {
        try
        {
            var details = await _profitService.GetDetailedProfit(userId);
            return Ok(details);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
