using CloudBank.Application.DTOs;
using CloudBank.Application.Interfaces;

namespace CloudBank.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<TokenResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository
            .GetByEmailAsync(request.Email);

        if (user == null)
        {
            return null;
        }

        var passwordValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            return null;
        }

        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role);

        return new TokenResponse
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}