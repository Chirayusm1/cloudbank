using CloudBank.Domain.Entities;

namespace CloudBank.Application.Interfaces;

public interface IAccountRepository
{
    Task<Account> AddAsync(Account account);

    Task<IEnumerable<Account>> GetAllAsync();

    Task<Account?> GetByIdAsync(Guid id);

    Task SaveChangesAsync();
}