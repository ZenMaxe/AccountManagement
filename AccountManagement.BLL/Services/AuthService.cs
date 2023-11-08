using System.Security.Claims;
using AccountManagement.Domain.DTOs;
using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Entities.Identity;
using AccountManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AccountManagement.BLL.Services;

public class AuthService : IAuthService
{

    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtHandler _jwtHandler;
    private readonly IHttpContextAccessor _httpContext;

    public AuthService(UserManager<AppUser> userManager,
                       IJwtHandler jwtHandler,
                       IHttpContextAccessor httpContext)
    {
        _userManager = userManager;
        _jwtHandler = jwtHandler;
        _httpContext = httpContext;
    }

    public async Task<TokenDto> LoginAsync(LoginDto loginDto)
    {
        if (!await ValidateUserAsync(loginDto))
            return null;

        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        var token = await _jwtHandler.CreateToken(user);

        return token;

    }

    public async Task<bool> ValidateUserAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        return (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password));
    }

    public async Task<AppUser?> GetUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null)
        {
            var user = await _userManager.FindByIdAsync(claim.Value);

            return user;
        }

        return null;
    }
    public Guid? GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null && Guid.TryParse(claim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }

    public Guid? GetUserIdFromClaims()
    {
        var claim = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null && Guid.TryParse(claim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }
}