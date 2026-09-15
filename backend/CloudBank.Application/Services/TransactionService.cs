using CloudBank.Application.DTOs;
using CloudBank.Application.Interfaces;
using CloudBank.Domain.Entities;

namespace CloudBank.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<TransactionResponse> CreateTransactionAsync(
        CreateTransactionRequest request)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("Transaction amount must be greater than zero.");

        if (request.FromAccountId == request.ToAccountId)
            throw new ArgumentException("From and To accounts must be different.");

        var fromAccount = await _accountRepository.GetByIdAsync(
            request.FromAccountId);

        var toAccount = await _accountRepository.GetByIdAsync(
            request.ToAccountId);

        if (fromAccount == null)
            throw new KeyNotFoundException("From account was not found.");

        if (toAccount == null)
            throw new KeyNotFoundException("To account was not found.");

        if (fromAccount.Status != "Active")
            throw new InvalidOperationException(
                "From account is not active.");

        if (toAccount.Status != "Active")
            throw new InvalidOperationException(
                "To account is not active.");

        if (fromAccount.Balance < request.Amount)
            throw new InvalidOperationException(
                "Insufficient balance.");

        fromAccount.Balance -= request.Amount;
        toAccount.Balance += request.Amount;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            FromAccountId = fromAccount.Id,
            ToAccountId = toAccount.Id,
            Amount = request.Amount,
            Type = request.Type,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction);

        await _transactionRepository.SaveChangesAsync();
        await _accountRepository.SaveChangesAsync();

        return MapToResponse(transaction);
    }

    public async Task<IEnumerable<TransactionResponse>>
        GetTransactionsByAccountIdAsync(Guid accountId)
    {
        var transactions =
            await _transactionRepository.GetByAccountIdAsync(accountId);

        return transactions.Select(MapToResponse);
    }

    public async Task<TransactionResponse?> GetTransactionByIdAsync(Guid id)
    {
        var transaction =
            await _transactionRepository.GetByIdAsync(id);

        return transaction == null
            ? null
            : MapToResponse(transaction);
    }

    private static TransactionResponse MapToResponse(
        Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Status = transaction.Status,
            CreatedAt = transaction.CreatedAt
        };
    }
}
