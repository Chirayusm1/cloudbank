namespace CloudBank.Application.DTOs;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}