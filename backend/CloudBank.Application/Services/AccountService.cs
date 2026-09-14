using CloudBank.Application.DTOs;
using CloudBank.Application.Interfaces;
using CloudBank.Domain.Entities;

namespace CloudBank.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRedisCacheService _redisCacheService;

    public AccountService(
        IAccountRepository accountRepository,
        IRedisCacheService redisCacheService)
    {
        _accountRepository = accountRepository;
        _redisCacheService = redisCacheService;
    }

    public async Task<AccountResponse> CreateAccountAsync(
        CreateAccountRequest request)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            AccountNumber = GenerateAccountNumber(),
            Balance = 0,
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        await _accountRepository.AddAsync(account);

        await _accountRepository.SaveChangesAsync();

        return MapToResponse(account);
    }

    public async Task<IEnumerable<AccountResponse>> GetAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();

        return accounts.Select(MapToResponse);
    }

    public async Task<AccountResponse?> GetAccountByIdAsync(Guid id)
    {
        var cacheKey = $"account:{id}";

        // 1. Check Redis first
        var cachedAccount =
            await _redisCacheService.GetAsync<AccountResponse>(cacheKey);

        if (cachedAccount is not null)
        {
            return cachedAccount;
        }

        // 2. Cache miss - get account from database
        var account = await _accountRepository.GetByIdAsync(id);

        if (account is null)
        {
            return null;
        }

        var response = MapToResponse(account);

        // 3. Store the response in Redis for 5 minutes
        await _redisCacheService.SetAsync(
            cacheKey,
            response,
            TimeSpan.FromMinutes(5));

        return response;
    }

    private static string GenerateAccountNumber()
    {
        return Random.Shared.NextInt64(
            1000000000,
            9999999999
        ).ToString();
    }

    private static AccountResponse MapToResponse(Account account)
    {
        return new AccountResponse
        {
            Id = account.Id,
            UserId = account.UserId,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };
    }
}
