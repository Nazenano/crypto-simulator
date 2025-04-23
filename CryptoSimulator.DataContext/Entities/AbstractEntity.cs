using System.ComponentModel.DataAnnotations;

namespace CryptoSimulator.DataContext.Entities;

public abstract class AbstractEntity
{
    [Key]
    public int Id { get; set; }
}
