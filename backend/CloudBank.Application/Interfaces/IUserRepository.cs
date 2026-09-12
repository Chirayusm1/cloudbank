using CloudBank.Domain.Entities;

namespace CloudBank.Application.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);

    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task SaveChangesAsync();
}