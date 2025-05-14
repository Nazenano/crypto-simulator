namespace CryptoSimulator.DataContext.Dtos;

public class TransactionDto
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    public string CryptoName { get; set; }
    public string CryptoSymbol { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
}

public class TransactionDetailsDto
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    public int UserId { get; set; }
    public string CryptoName { get; set; }
    public string CryptoSymbol { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
}
