using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.Services;

public interface IWalletService
{
    Task<WalletDto> Get(int userId);
    Task<WalletDto> Update(int userId, WalletUpdateDto dto);
    Task Delete(int userId);
}

public class WalletService : IWalletService
{
    private readonly SQL _context;
    private readonly IMapper _mapper;

    public WalletService(SQL context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WalletDto> Get(int userId)
    {
        Wallet? getWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (getWallet == null)
        {
            throw new Exception("Wallet does not exist!");
        }

        List<Portfolio> portfolios = await _context.Portfolios
            .Where(p => p.UserId == userId)
            .Include(p => p.CryptoCurrency)
            .ToListAsync();

        WalletDto walletDto = _mapper.Map<WalletDto>(getWallet);
        walletDto.Cryptocurrencies = _mapper.Map<List<PortfolioDto>>(portfolios);

        return walletDto;
    }

    public async Task<WalletDto> Update(int userId, WalletUpdateDto wallet)
    {
        Wallet? getWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (getWallet == null)
        {
            throw new Exception("Wallet does not exist!");
        }
        if (wallet.Balance < 0)
        {
            throw new Exception("Balance cannot be negative!");
        }

        _mapper.Map(wallet, getWallet);
        await _context.SaveChangesAsync();

        return _mapper.Map<WalletDto>(getWallet);
    }

    public async Task Delete(int userId)
    {
        Wallet? getWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (getWallet == null)
        {
            throw new Exception("Wallet does not exist!");
        }

        _context.Wallets.Remove(getWallet);
        await _context.SaveChangesAsync();
    }
}
