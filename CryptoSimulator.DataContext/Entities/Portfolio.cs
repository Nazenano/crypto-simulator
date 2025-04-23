using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoSimulator.DataContext.Entities;

public class Portfolio : AbstractEntity
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int CryptoId { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    // Navigation props
    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("CryptoId")]
    public CryptoCurrency CryptoCurrency { get; set; }
}
