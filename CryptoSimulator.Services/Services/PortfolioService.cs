using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.Services;

public interface IPortfolioService
{
    Task<List<PortfolioDto>> GetAll(int userId);
}

public class PortfolioService : IPortfolioService
{
    private readonly SQL _context;
    private readonly IMapper _mapper;

    public PortfolioService(SQL context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PortfolioDto>> GetAll(int userId)
    {
        List<Portfolio> portfolios = await _context.Portfolios.Include(p => p.CryptoCurrency).Where(p => p.UserId == userId).ToListAsync();
        return _mapper.Map<List<PortfolioDto>>(portfolios);
    }
}
