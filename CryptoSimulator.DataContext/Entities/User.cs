using System.ComponentModel.DataAnnotations;

namespace CryptoSimulator.DataContext.Entities;

public enum UserRole
{
    User = 0,
    Admin = 1
}

public class User : AbstractEntity
{
    [Required]
    public string Username { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    // Navigation props
    public Wallet Wallet { get; set; }
    public ICollection<Portfolio> Portfolios { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}
