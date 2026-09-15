using CloudBank.Application.DTOs;

namespace CloudBank.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> CreateTransactionAsync(
        CreateTransactionRequest request);

    Task<IEnumerable<TransactionResponse>> GetTransactionsByAccountIdAsync(
        Guid accountId);

    Task<TransactionResponse?> GetTransactionByIdAsync(
        Guid id);
}