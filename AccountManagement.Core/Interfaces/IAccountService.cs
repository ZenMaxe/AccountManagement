using AccountManagement.Domain.DTOs;
using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Responses;

namespace AccountManagement.Domain.Interfaces;

public interface IAccountService
{
    Task<ApiResult<UserDto>> CreateUser(CreateUserDto createUserDto);
    Task<ApiResult<TokenDto>> LoginUser(LoginDto loginDto);
    Task<ApiResult<UserDto>> GetUserByIdAsync(Guid userId);
    Task<ApiResult<string>> ChangePasswordAsync(Guid userId, UserChangePasswordDto model);
    Task<ApiResult<string>> ValidateToken(UserVerifyTokenDto model);
    Task<ApiResult<string>> ForgetPassword(UserBase model);
    Task<ApiResult<UserDto>> EditUserDetails(Guid userId, EditUserDto model);
    Task<ApiResult<UserDto>> UserFullDetails();
    Task<bool> IsUsernameFree(string username);
}