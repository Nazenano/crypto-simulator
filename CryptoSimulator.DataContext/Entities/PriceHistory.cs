using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoSimulator.DataContext.Entities;

public class PriceHistory : AbstractEntity
{
    [Required]
    public int CryptoId { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation prop
    [ForeignKey("CryptoId")]
    public CryptoCurrency CryptoCurrency { get; set; }
}
