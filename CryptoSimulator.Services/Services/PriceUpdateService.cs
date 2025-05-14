using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Dtos;
using CryptoSimulator.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CryptoSimulator.Services;

public interface IPriceUpdateService
{
    Task UpdatePrice(PriceUpdateDto dto);
    Task<IEnumerable<PriceHistoryDto>> GetPriceHistory(int cryptoId);
}

public class PriceUpdateService : BackgroundService, IPriceUpdateService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PriceUpdateService> _logger;
    private readonly TimeSpan _updateInterval;
    private static readonly Random _random = new();
    private readonly IMapper _mapper;

    public PriceUpdateService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<PriceUpdateService> logger,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;

        string intervalString = _configuration["PriceUpdateIntervalSeconds"];
        int intervalSeconds = int.TryParse(intervalString, out int parsed) ? parsed : 60;
        _updateInterval = TimeSpan.FromSeconds(intervalSeconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SQL>();

                var cryptos = await context.CryptoCurrencies.ToListAsync(stoppingToken);
                foreach (var crypto in cryptos)
                {
                    decimal oldPrice = crypto.Price;
                    decimal percentageChange = (decimal)(_random.NextDouble() * 2 - 1) * 0.05m; // ±5%
                    decimal newPrice = Math.Max(0.01m, crypto.Price * (1 + percentageChange));
                    newPrice = Math.Round(newPrice, 2);

                    crypto.Price = newPrice;
                    context.PriceHistories.Add(new PriceHistory
                    {
                        CryptoId = crypto.Id,
                        Price = newPrice,
                        Timestamp = DateTime.UtcNow
                    });
                }

                await context.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Updated prices for {Count} cryptocurrencies.", cryptos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prices.");
            }

            await Task.Delay(_updateInterval, stoppingToken);
        }
    }

    public async Task UpdatePrice(PriceUpdateDto dto)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SQL>();

        var crypto = await context.CryptoCurrencies
            .FirstOrDefaultAsync(c => c.Id == dto.CryptoId);
        if (crypto == null)
        {
            throw new Exception("Cryptocurrency not found!");
        }

        crypto.Price = dto.NewPrice;
        context.PriceHistories.Add(new PriceHistory
        {
            CryptoId = dto.CryptoId,
            Price = dto.NewPrice,
            Timestamp = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PriceHistoryDto>> GetPriceHistory(int cryptoId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SQL>();

        var history = await context.PriceHistories
            .Where(h => h.CryptoId == cryptoId)
            .OrderByDescending(h => h.Timestamp)
            .ToListAsync();

        if (history == null || !history.Any())
        {
            throw new Exception("No price history found for this cryptocurrency!");
        }

        return _mapper.Map<IEnumerable<PriceHistoryDto>>(history);
    }
}
