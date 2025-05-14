using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSimulator.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateDto user)
    {
        try
        {
            UserDto newUser = await _userService.Register(user);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto user)
    {
        try
        {
            string token = await _userService.Login(user);
            return Ok(new { token });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpGet("{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> Get(int userId)
    {
        try
        {
            UserDto user = await _userService.Get(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPut("{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> Update(int userId, [FromBody] UserUpdateDto user)
    {
        try
        {
            UserDto getUser = await _userService.Update(userId, user);
            return Ok(getUser);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPut("password/change/{userId}")]
    // [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> UpdatePassword(int userId, [FromBody] UserPassUpdateDto user)
    {
        try
        {
            await _userService.UpdatePassword(userId, user);
            return NoContent();
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
            await _userService.Delete(userId);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(new { error = e.Message });
        }
    }
}
