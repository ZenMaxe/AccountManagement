using AccountManagement.Domain.DTOs;
using AccountManagement.Domain.Entities.Identity;

namespace AccountManagement.Domain.Interfaces;

public interface IJwtHandler
{
    Task<TokenDto> CreateToken(AppUser user);
    TokenDto RefreshToken(string expiredToken, string refreshToken);
}