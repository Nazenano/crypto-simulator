using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.Services;

public interface ITradeService
{
    Task<List<TradeDto>> Get(int userId);
    Task<TradeDto> Buy(int userId, TradeCreateDto dto);
    Task<TradeDto> Sell(int userId, TradeCreateDto dto);
}

public class TradeService : ITradeService
{
    private readonly SQL _context;
    private readonly IMapper _mapper;

    public TradeService(SQL context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TradeDto>> Get(int userId)
    {
        List<Transaction> transactions = await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
        return _mapper.Map<List<TradeDto>>(transactions);
    }

    public async Task<TradeDto> Buy(int userId, TradeCreateDto buy)
    {
        if (buy.Quantity <= 0)
        {
            throw new Exception("Quantity must be positive!");
        }


        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }

        CryptoCurrency? getCrypto = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.Id == buy.CryptoId);
        if (getCrypto == null)
        {
            throw new Exception("Crypto does not exist!");
        }
        if (buy.Quantity > getCrypto.TotalSupply)
        {
            throw new Exception("Cannot buy more than the total supply!");
        }

        Wallet? getWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (getWallet == null)
        {
            throw new Exception("Wallet does not exist!");
        }

        decimal totalCost = buy.Quantity * getCrypto.Price;
        if (getWallet.Balance < totalCost)
        {
            throw new Exception("Insufficient balance!");
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                getWallet.Balance -= totalCost;
                getCrypto.TotalSupply -= buy.Quantity;

                var newTransaction = new Transaction
                {
                    UserId = userId,
                    CryptoId = buy.CryptoId,
                    Quantity = buy.Quantity,
                    Price = getCrypto.Price,
                    Type = TransactionType.Buy,
                    Timestamp = DateTime.UtcNow
                };
                await _context.Transactions.AddAsync(newTransaction);

                var portfolio = await _context.Portfolios
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.CryptoId == buy.CryptoId);
                if (portfolio == null)
                {
                    portfolio = new Portfolio
                    {
                        UserId = userId,
                        CryptoId = buy.CryptoId,
                        Quantity = buy.Quantity
                    };
                    await _context.Portfolios.AddAsync(portfolio);
                }
                else
                {
                    portfolio.Quantity += buy.Quantity;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<TradeDto>(newTransaction);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<TradeDto> Sell(int userId, TradeCreateDto sell)
    {
        if (sell.Quantity <= 0)
        {
            throw new Exception("Quantity must be positive!");
        }

        User? getUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (getUser == null)
        {
            throw new Exception("User does not exist!");
        }

        CryptoCurrency? getCrypto = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.Id == sell.CryptoId);
        if (getCrypto == null)
        {
            throw new Exception("Crypto does not exist!");
        }

        Portfolio? getPortfolio = await _context.Portfolios
            .FirstOrDefaultAsync(p => p.UserId == userId && p.CryptoId == sell.CryptoId);
        if (getPortfolio == null || getPortfolio.Quantity < sell.Quantity)
        {
            throw new Exception("Insufficient crypto quantity!");
        }

        Wallet? getWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (getWallet == null)
        {
            throw new Exception("Wallet does not exist!");
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                decimal totalValue = sell.Quantity * getCrypto.Price;
                getWallet.Balance += totalValue;
                getPortfolio.Quantity -= sell.Quantity;
                getCrypto.TotalSupply += sell.Quantity;

                if (getPortfolio.Quantity == 0)
                    _context.Portfolios.Remove(getPortfolio);

                var newTransaction = new Transaction
                {
                    UserId = userId,
                    CryptoId = sell.CryptoId,
                    Quantity = sell.Quantity,
                    Price = getCrypto.Price,
                    Type = TransactionType.Sell,
                    Timestamp = DateTime.UtcNow
                };
                await _context.Transactions.AddAsync(newTransaction);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<TradeDto>(newTransaction);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
