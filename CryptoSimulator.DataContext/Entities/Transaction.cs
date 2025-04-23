using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoSimulator.DataContext.Entities;

public enum TransactionType
{
    Buy = 0,
    Sell = 1
}

public class Transaction : AbstractEntity
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int CryptoId { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation props
    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("CryptoId")]
    public CryptoCurrency CryptoCurrency { get; set; }
}
