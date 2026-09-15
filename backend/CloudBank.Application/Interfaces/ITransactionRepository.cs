namespace CloudBank.Application.Interfaces;

using CloudBank.Domain.Entities;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction);

    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);

    Task<Transaction?> GetByIdAsync(Guid id);

    Task SaveChangesAsync();
}