using CloudBank.Application.DTOs;

namespace CloudBank.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);

    Task<UserResponse?> GetUserByIdAsync(Guid id);
}