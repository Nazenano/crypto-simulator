namespace CryptoSimulator.DataContext.Dtos;

public class WalletDto
{
    public int UserId { get; set; }
    public decimal Balance { get; set; }
    public List<PortfolioDto> Cryptocurrencies { get; set; } = new List<PortfolioDto>();
}

public class PortfolioDto
{
    public int CryptoId { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal Quantity { get; set; }
}

public class WalletUpdateDto
{
    public decimal Balance { get; set; }
}
