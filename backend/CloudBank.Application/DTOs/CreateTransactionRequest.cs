
namespace CloudBank.Application.DTOs;

public class CreateTransactionRequest
{
    public Guid FromAccountId { get; set; }

    public Guid ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;
}

