using CloudBank.Application.DTOs;

namespace CloudBank.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginRequest request);
}