using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.Services;

public interface IProfitService
{
    Task<ProfitDto> GetTotalProfit(int userId);
    Task<IEnumerable<ProfitDetailDto>> GetDetailedProfit(int userId);
}

public class ProfitService : IProfitService
{
    private readonly SQL _context;

    public ProfitService(SQL context)
    {
        _context = context;
    }

    public async Task<ProfitDto> GetTotalProfit(int userId)
    {
        var portfolios = await _context.Portfolios
            .Where(p => p.UserId == userId)
            .Include(p => p.CryptoCurrency)
            .ToListAsync();

        if (portfolios == null || !portfolios.Any())
            return new ProfitDto { TotalProfit = 0 };

        decimal totalProfit = 0;
        foreach (var portfolio in portfolios)
        {
            var purchasePrice = await CalculateAveragePurchasePriceAsync(userId, portfolio.CryptoId);
            var currentPrice = portfolio.CryptoCurrency.Price;
            var profit = (currentPrice - purchasePrice) * portfolio.Quantity;
            totalProfit += profit;
        }

        return new ProfitDto { TotalProfit = Math.Round(totalProfit, 2) };
    }

    public async Task<IEnumerable<ProfitDetailDto>> GetDetailedProfit(int userId)
    {
        var portfolios = await _context.Portfolios
            .Where(p => p.UserId == userId)
            .Include(p => p.CryptoCurrency)
            .ToListAsync();

        if (portfolios == null || !portfolios.Any())
            return Enumerable.Empty<ProfitDetailDto>();

        var details = new List<ProfitDetailDto>();
        foreach (var portfolio in portfolios)
        {
            var purchasePrice = await CalculateAveragePurchasePriceAsync(userId, portfolio.CryptoId);
            var currentPrice = portfolio.CryptoCurrency.Price;
            var profit = (currentPrice - purchasePrice) * portfolio.Quantity;

            details.Add(new ProfitDetailDto
            {
                CryptoId = portfolio.CryptoId,
                Name = portfolio.CryptoCurrency.Name,
                Symbol = portfolio.CryptoCurrency.Symbol,
                Quantity = portfolio.Quantity,
                PurchasePrice = Math.Round(purchasePrice, 2),
                Price = Math.Round(currentPrice, 2),
                Profit = Math.Round(profit, 2)
            });
        }

        return details;
    }

    private async Task<decimal> CalculateAveragePurchasePriceAsync(int userId, int cryptoId)
    {
        var buyTransactions = await _context.Transactions
            .Where(t => t.UserId == userId && t.CryptoId == cryptoId && t.Type == TransactionType.Buy)
            .ToListAsync();

        if (buyTransactions == null || !buyTransactions.Any())
            return 0;

        decimal totalCost = 0;
        decimal totalQuantity = 0;
        foreach (var transaction in buyTransactions)
        {
            totalCost += transaction.Price * transaction.Quantity;
            totalQuantity += transaction.Quantity;
        }

        return totalQuantity > 0 ? totalCost / totalQuantity : 0;
    }
}
