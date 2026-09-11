namespace CloudBank.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string Status { get; set; } = "Active";

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Transaction> OutgoingTransactions { get; set; }
        = new List<Transaction>();

    public ICollection<Transaction> IncomingTransactions { get; set; }
        = new List<Transaction>();
}