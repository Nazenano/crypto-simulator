namespace CryptoSimulator.DataContext.Dtos;

public class CryptoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public decimal TotalSupply { get; set; }
}

public class CryptoCreateDto
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public decimal TotalSupply { get; set; }
}

public class CryptoBuySellDto
{
    public int CryptoId { get; set; }
    public decimal Quantity { get; set; }
}
