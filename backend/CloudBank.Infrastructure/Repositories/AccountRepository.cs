using CloudBank.Application.Interfaces;
using CloudBank.Domain.Entities;
using CloudBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CloudBank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly CloudBankDbContext _dbContext;

    public AccountRepository(CloudBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account> AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);

        return account;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _dbContext.Accounts
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}