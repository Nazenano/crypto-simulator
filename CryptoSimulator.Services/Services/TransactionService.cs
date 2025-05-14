using AutoMapper;
using CryptoSimulator.DataContext;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> Get(int userId);
    Task<TransactionDetailsDto> GetDetails(int transactionId);
}

public class TransactionService : ITransactionService
{
    private readonly SQL _context;
    private readonly IMapper _mapper;

    public TransactionService(SQL context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> Get(int userId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.CryptoCurrency)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();

        if (transactions == null || !transactions.Any())
        {
            return Enumerable.Empty<TransactionDto>();
        }

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDetailsDto> GetDetails(int transactionId)
    {
        var transaction = await _context.Transactions
            .Include(t => t.CryptoCurrency)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
        {
            throw new Exception("Transaction not found!");
        }

        return _mapper.Map<TransactionDetailsDto>(transaction);
    }
}
