namespace CryptoSimulator.DataContext.Dtos;

public class PriceUpdateDto
{
    public int CryptoId { get; set; }
    public decimal NewPrice { get; set; }
}

public class PriceHistoryDto
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ProfitDto
{
    public decimal TotalProfit { get; set; }
}

public class ProfitDetailDto
{
    public int CryptoId { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public decimal Profit { get; set; }
}
