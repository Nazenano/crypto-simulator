namespace CryptoSimulator.DataContext.Dtos;

public class TradeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CryptoId { get; set; }
    public string Type { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}

public class TradeCreateDto
{
    public int CryptoId { get; set; }
    public decimal Quantity { get; set; }
}
