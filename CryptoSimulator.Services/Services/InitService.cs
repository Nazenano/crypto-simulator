using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CryptoSimulator.Services;

public interface IInitService
{
    Task SeedDatabaseAsync();
}

public class InitService : IInitService
{
    private readonly SQL _context;

    public InitService(SQL context)
    {
        _context = context;
    }

    public async Task SeedDatabaseAsync()
    {
        // Check if ANY data exists in key tables
        bool hasData = await _context.Users.AnyAsync() ||
                       await _context.Wallets.AnyAsync() ||
                       await _context.CryptoCurrencies.AnyAsync() ||
                       await _context.Portfolios.AnyAsync() ||
                       await _context.Transactions.AnyAsync() ||
                       await _context.PriceHistories.AnyAsync();

        if (hasData)
            return; // Skip seeding if any table has data

        // Seed Users
        var users = new List<User>
        {
            new User
            {
                Username = "john_doe",
                Email = "john@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = UserRole.User
            },
            new User
            {
                Username = "jane_smith",
                Email = "jane@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = UserRole.User
            },
            new User
            {
                Username = "admin",
                Email = "admin@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin
            }
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Seed Wallets
        var wallets = new List<Wallet>
        {
            new Wallet { UserId = 1, Balance = 10000.00m },
            new Wallet { UserId = 2, Balance = 10000.00m },
            new Wallet { UserId = 3, Balance = 10000.00m }
        };
        await _context.Wallets.AddRangeAsync(wallets);

        // Seed CryptoCurrencies
        var cryptos = new List<CryptoCurrency>
        {
            new CryptoCurrency { Name = "Bitcoin", Symbol = "BTC", Price = 51000.00m, TotalSupply = 21000000.00m },
            new CryptoCurrency { Name = "Ethereum", Symbol = "ETH", Price = 3100.00m, TotalSupply = 120000000.00m },
            new CryptoCurrency { Name = "Binance Coin", Symbol = "BNB", Price = 520.00m, TotalSupply = 200000000.00m },
            new CryptoCurrency { Name = "Cardano", Symbol = "ADA", Price = 1.25m, TotalSupply = 45000000000.00m },
            new CryptoCurrency { Name = "Solana", Symbol = "SOL", Price = 155.00m, TotalSupply = 500000000.00m },
            new CryptoCurrency { Name = "Ripple", Symbol = "XRP", Price = 0.95m, TotalSupply = 100000000000.00m },
            new CryptoCurrency { Name = "Polkadot", Symbol = "DOT", Price = 26.00m, TotalSupply = 1100000000.00m },
            new CryptoCurrency { Name = "Dogecoin", Symbol = "DOGE", Price = 0.22m, TotalSupply = 132000000000.00m },
            new CryptoCurrency { Name = "Avalanche", Symbol = "AVAX", Price = 62.00m, TotalSupply = 720000000.00m },
            new CryptoCurrency { Name = "Polygon", Symbol = "MATIC", Price = 1.60m, TotalSupply = 10000000000.00m },
            new CryptoCurrency { Name = "Cosmos", Symbol = "ATOM", Price = 32.00m, TotalSupply = 390000000.00m },
            new CryptoCurrency { Name = "Chainlink", Symbol = "LINK", Price = 23.00m, TotalSupply = 1000000000.00m },
            new CryptoCurrency { Name = "Algorand", Symbol = "ALGO", Price = 0.85m, TotalSupply = 10000000000.00m },
            new CryptoCurrency { Name = "Stellar", Symbol = "XLM", Price = 0.32m, TotalSupply = 50000000000.00m },
            new CryptoCurrency { Name = "Tezos", Symbol = "XTZ", Price = 4.20m, TotalSupply = 1000000000.00m }
        };
        await _context.CryptoCurrencies.AddRangeAsync(cryptos);
        await _context.SaveChangesAsync();

        // Seed Portfolios
        var portfolios = new List<Portfolio>
        {
            new Portfolio { UserId = 1, CryptoId = 1, Quantity = 0.5m },
            new Portfolio { UserId = 1, CryptoId = 2, Quantity = 10.0m },
            new Portfolio { UserId = 2, CryptoId = 3, Quantity = 50.0m },
            new Portfolio { UserId = 2, CryptoId = 4, Quantity = 100.0m }
        };
        await _context.Portfolios.AddRangeAsync(portfolios);

        // Seed Transactions
        var transactions = new List<Transaction>
        {
            new Transaction { UserId = 1, CryptoId = 1, Quantity = 0.5m, Price = 50000.00m, Type = TransactionType.Buy, Timestamp = DateTime.Parse("2025-04-27 10:00:00") },
            new Transaction { UserId = 1, CryptoId = 2, Quantity = 10.0m, Price = 3000.00m, Type = TransactionType.Buy, Timestamp = DateTime.Parse("2025-04-27 10:01:00") },
            new Transaction { UserId = 2, CryptoId = 3, Quantity = 50.0m, Price = 500.00m, Type = TransactionType.Buy, Timestamp = DateTime.Parse("2025-04-27 10:02:00") },
            new Transaction { UserId = 2, CryptoId = 4, Quantity = 100.0m, Price = 1.20m, Type = TransactionType.Buy, Timestamp = DateTime.Parse("2025-04-27 10:03:00") },
            new Transaction { UserId = 1, CryptoId = 1, Quantity = 0.1m, Price = 51000.00m, Type = TransactionType.Sell, Timestamp = DateTime.Parse("2025-04-27 11:00:00") },
            new Transaction { UserId = 2, CryptoId = 3, Quantity = 10.0m, Price = 520.00m, Type = TransactionType.Sell, Timestamp = DateTime.Parse("2025-04-27 11:01:00") }
        };
        await _context.Transactions.AddRangeAsync(transactions);

        // Seed PriceHistories
        var priceHistories = new List<PriceHistory>
        {
            new PriceHistory { CryptoId = 1, Price = 50000.00m, Timestamp = DateTime.Parse("2025-04-27 09:00:00") },
            new PriceHistory { CryptoId = 1, Price = 51000.00m, Timestamp = DateTime.Parse("2025-04-27 10:00:00") },
            new PriceHistory { CryptoId = 2, Price = 3000.00m, Timestamp = DateTime.Parse("2025-04-27 09:00:00") },
            new PriceHistory { CryptoId = 2, Price = 3100.00m, Timestamp = DateTime.Parse("2025-04-27 10:00:00") },
            new PriceHistory { CryptoId = 3, Price = 500.00m, Timestamp = DateTime.Parse("2025-04-27 09:00:00") },
            new PriceHistory { CryptoId = 3, Price = 520.00m, Timestamp = DateTime.Parse("2025-04-27 10:00:00") },
            new PriceHistory { CryptoId = 4, Price = 1.20m, Timestamp = DateTime.Parse("2025-04-27 09:00:00") },
            new PriceHistory { CryptoId = 4, Price = 1.25m, Timestamp = DateTime.Parse("2025-04-27 10:00:00") }
        };
        await _context.PriceHistories.AddRangeAsync(priceHistories);

        await _context.SaveChangesAsync();
    }
}
