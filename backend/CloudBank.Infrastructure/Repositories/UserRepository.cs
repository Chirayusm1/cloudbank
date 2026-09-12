using CloudBank.Application.Interfaces;
using CloudBank.Domain.Entities;
using CloudBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CloudBank.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CloudBankDbContext _dbContext;

    public UserRepository(CloudBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);

        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}