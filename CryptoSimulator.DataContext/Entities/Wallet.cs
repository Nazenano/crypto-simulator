using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoSimulator.DataContext.Entities;

public class Wallet : AbstractEntity
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public decimal Balance { get; set; }

    // Navigation prop
    [ForeignKey("UserId")]
    public User User { get; set; }
}
