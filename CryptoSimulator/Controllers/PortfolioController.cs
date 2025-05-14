using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpGet("{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(int userId)
    {
        try
        {
            var portfolio = await _portfolioService.GetAll(userId);
            return Ok(portfolio);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
