using System.Security.Claims;
using AccountManagement.Domain.DTOs;
using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Entities.Identity;

namespace AccountManagement.Domain.Interfaces;

public interface IAuthService
{
    Task<TokenDto> LoginAsync(LoginDto loginDto);
    Task<bool> ValidateUserAsync(LoginDto loginDto);
    Task<AppUser?> GetUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
    Guid? GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal);
    Guid? GetUserIdFromClaims();
}