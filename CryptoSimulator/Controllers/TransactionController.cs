using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> Get(int userId)
    {
        try
        {
            var transactions = await _transactionService.Get(userId);
            return Ok(transactions);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("details/{transactionId}")]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<TransactionDto>> GetDetails(int transactionId)
    {
        try
        {
            var transaction = await _transactionService.GetDetails(transactionId);
            return Ok(transaction);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}
