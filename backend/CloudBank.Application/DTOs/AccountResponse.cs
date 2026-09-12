namespace CloudBank.Application.DTOs;

public class AccountResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}