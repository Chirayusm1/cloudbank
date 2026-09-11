namespace CloudBank.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public Guid FromAccountId { get; set; }

    public Guid ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Account FromAccount { get; set; } = null!;

    public Account ToAccount { get; set; } = null!;
}