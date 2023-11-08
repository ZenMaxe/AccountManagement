namespace AccountManagement.Domain.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiry { get; set; }
}