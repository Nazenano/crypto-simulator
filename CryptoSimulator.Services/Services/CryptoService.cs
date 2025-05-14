using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.Services;

public interface ICryptoService
{
    Task<List<CryptoDto>> GetAll();
    Task<CryptoDto> Get(int id);
    Task<CryptoDto> Create(CryptoCreateDto dto);
    Task<CryptoDto> Update(int id, CryptoCreateDto dto);
    Task Delete(int id);
}

public class CryptoService : ICryptoService
{
    private readonly SQL _context;
    private readonly IMapper _mapper;

    public CryptoService(SQL context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CryptoDto>> GetAll()
    {
        var cryptos = await _context.CryptoCurrencies.ToListAsync();
        return _mapper.Map<List<CryptoDto>>(cryptos);
    }

    public async Task<CryptoDto> Get(int id)
    {
        CryptoCurrency? getCrypto = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.Id == id);
        if (getCrypto == null)
        {
            throw new Exception("Crypto does not exist!");
        }

        return _mapper.Map<CryptoDto>(getCrypto);
    }

    public async Task<CryptoDto> Create(CryptoCreateDto crypto)
    {
        if (crypto.Price < 0)
        {
            throw new Exception("Price cannot be negative!");
        }
        if (await _context.CryptoCurrencies.AnyAsync(c => c.Symbol == crypto.Symbol))
        {
            throw new Exception("Crypto symbol already exists!");
        }
        if (crypto.TotalSupply <= 0)
        {
            throw new Exception("Total supply must be positive!");
        }

        CryptoCurrency newCrypto = _mapper.Map<CryptoCurrency>(crypto);
        await _context.CryptoCurrencies.AddAsync(newCrypto);
        await _context.SaveChangesAsync();

        return _mapper.Map<CryptoDto>(newCrypto);
    }

    public async Task<CryptoDto> Update(int id, CryptoCreateDto crypto)
    {
        CryptoCurrency? getCrypto = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.Id == id);
        if (getCrypto == null)
        {
            throw new Exception("Crypto does not exist!");
        }
        if (crypto.Price < 0)
        {
            throw new Exception("Price cannot be negative!");
        }
        if (await _context.CryptoCurrencies.AnyAsync(c => c.Symbol == crypto.Symbol && c.Id != id))
        {
            throw new Exception("Crypto symbol already exists!");
        }

        _mapper.Map(crypto, getCrypto);
        await _context.SaveChangesAsync();

        return _mapper.Map<CryptoDto>(getCrypto);
    }

    public async Task Delete(int id)
    {
        CryptoCurrency? getCrypto = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.Id == id);
        if (getCrypto == null)
        {
            throw new Exception("Crypto does not exist!");
        }

        _context.CryptoCurrencies.Remove(getCrypto);
        await _context.SaveChangesAsync();
    }
}
