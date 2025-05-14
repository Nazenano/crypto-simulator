using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CryptoSimulator.Services;

public interface IUserService
{
    Task<UserDto> Register(UserCreateDto dto);
    Task<string> Login(UserLoginDto dto);
    Task<UserDto> Get(int id);
    Task<UserDto> Update(int id, UserUpdateDto dto);
    Task UpdatePassword(int id, UserPassUpdateDto dto);
    Task Delete(int id);
}

public class UserService : IUserService
{
    private readonly SQL _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserService(SQL context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<UserDto> Register(UserCreateDto user)
    {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.PasswordConfirm))
        {
            throw new Exception("All fields are required!");
        }
        if (user.Password != user.PasswordConfirm)
        {
            throw new Exception("Passwords do not match!");
        }
        if (user.Password.Length < 8)
        {
            throw new Exception("Password must be at least 8 characters long!");
        }

        User newUser = _mapper.Map<User>(user);
        if (await _context.Users.AnyAsync(u => u.Email == newUser.Email) || await _context.Users.AnyAsync(u => u.Username == newUser.Username))
        {
            throw new Exception("User already exists!");
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // add wallet
                Wallet wallet = new Wallet { UserId = newUser.Id, Balance = 10000m };
                await _context.Wallets.AddAsync(wallet);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return _mapper.Map<UserDto>(newUser);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception($"Server error: {e.Message}");
            }
        }
    }

    public async Task<string> Login(UserLoginDto user)
    {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            throw new Exception("All fields are required!");
        }

        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }

        if (!BCrypt.Net.BCrypt.Verify(user.Password, getUser.Password))
        {
            throw new Exception("Incorrect password!");
        }

        return await GenerateToken(getUser);
    }

    // helpers

    private async Task<string> GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var id = await GetClaimsIdentity(user);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: id.Claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture))
            };

        return new ClaimsIdentity(claims, "Token");
    }
    //

    public async Task<UserDto> Get(int id)
    {
        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }

        return _mapper.Map<UserDto>(getUser);
    }

    public async Task<UserDto> Update(int id, UserUpdateDto user)
    {
        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }
        if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != id))
        {
            throw new Exception("Email already exists!");
        }

        _mapper.Map(user, getUser);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(getUser);
    }

    public async Task UpdatePassword(int id, UserPassUpdateDto user)
    {
        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }
        if (!BCrypt.Net.BCrypt.Verify(user.Password, getUser.Password))
        {
            throw new Exception("Current password is incorrect!");
        }
        if (user.NewPassword.Length < 8)
        {
            throw new Exception("Password must be at least 8 characters long!");
        }
        if (!user.NewPassword.Equals(user.NewPasswordConfirm))
        {
            throw new Exception("New passwords do not match!");
        }
        if (user.NewPassword.Equals(user.Password))
        {
            throw new Exception("Current password and new password should not match!");
        }

        getUser.Password = BCrypt.Net.BCrypt.HashPassword(user.NewPassword);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }

        _context.Users.Remove(getUser);
        await _context.SaveChangesAsync();
    }
}
