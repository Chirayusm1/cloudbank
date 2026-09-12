using CloudBank.Application.DTOs;

namespace CloudBank.Application.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> CreateAccountAsync(CreateAccountRequest request);

    Task<IEnumerable<AccountResponse>> GetAccountsAsync();

    Task<AccountResponse?> GetAccountByIdAsync(Guid id);
}