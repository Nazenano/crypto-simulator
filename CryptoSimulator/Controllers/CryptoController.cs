using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[Route("api/cryptos")]
[ApiController]
public class CryptosController : ControllerBase
{
    private readonly ICryptoService _cryptoService;

    public CryptosController(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<CryptoDto> cryptos = await _cryptoService.GetAll();
            return Ok(cryptos);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpGet("{cryptoId}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int cryptoId)
    {
        try
        {
            CryptoDto crypto = await _cryptoService.Get(cryptoId);
            return Ok(crypto);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create([FromBody] CryptoCreateDto dto)
    {
        try
        {
            CryptoDto crypto = await _cryptoService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = crypto.Id }, crypto);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPut("{cryptoId}")]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Update(int cryptoId, [FromBody] CryptoCreateDto crypto)
    {
        try
        {
            CryptoDto getCrypto = await _cryptoService.Update(cryptoId, crypto);
            return Ok(getCrypto);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpDelete("{cryptoId}")]
    // [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(int cryptoId)
    {
        try
        {
            await _cryptoService.Delete(cryptoId);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}
