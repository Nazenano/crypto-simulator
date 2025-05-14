using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[Route("api/wallet")]
[ApiController]
public class WalletsController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletsController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> Get(int userId)
    {
        try
        {
            WalletDto wallet = await _walletService.Get(userId);
            return Ok(wallet);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPut("{userId}")]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Update(int userId, [FromBody] WalletUpdateDto wallet)
    {
        try
        {
            WalletDto getWallet = await _walletService.Update(userId, wallet);
            return Ok(getWallet);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpDelete("{userId}")]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(int userId)
    {
        try
        {
            await _walletService.Delete(userId);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}
