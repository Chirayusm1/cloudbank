using CloudBank.Application.Interfaces;
using CloudBank.Domain.Entities;
using CloudBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CloudBank.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly CloudBankDbContext _dbContext;

    public TransactionRepository(CloudBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);

        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbContext.Transactions
            .AsNoTracking()
            .Where(x =>
                x.FromAccountId == accountId ||
                x.ToAccountId == accountId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
