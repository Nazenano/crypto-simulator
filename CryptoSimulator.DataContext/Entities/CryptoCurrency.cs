using System.ComponentModel.DataAnnotations;

namespace CryptoSimulator.DataContext.Entities;

public class CryptoCurrency : AbstractEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Symbol { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public decimal TotalSupply { get; set; }

    // Navigation props
    public ICollection<Portfolio> Portfolios { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<PriceHistory> PriceHistories { get; set; }
}
