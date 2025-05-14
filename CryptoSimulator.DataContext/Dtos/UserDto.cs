using CryptoSimulator.DataContext.Entities;

namespace CryptoSimulator.DataContext.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}

public class UserCreateDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public UserRole Role { get; set; }
}

public class UserUpdateDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}

public class UserPassUpdateDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirm { get; set; }
}

public class UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
