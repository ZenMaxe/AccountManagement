using AccountManagement.Domain.DTOs;
using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Entities.Identity;
using AccountManagement.Domain.Interfaces;
using AccountManagement.Domain.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AccountManagement.BLL.Services;

public class AccountService : IAccountService, IUserService
{
    readonly IAuthService _authService;
    readonly IJwtHandler _jwtHandler;
    private readonly UserManager<AppUser> _userManager;
    readonly ILogger<AccountService> _logger;
    readonly IMapper _mapper;

    public AccountService(IAuthService authService, UserManager<AppUser> userManager,
                          IMapper mapper, ILogger<AccountService> logger,
                          IJwtHandler jwtHandler)
    {
        _authService = authService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _jwtHandler = jwtHandler;
    }

    public async Task<ApiResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<AppUser>(createUserDto);

        if (user is null)
        {
            _logger.LogCritical($"Can't Map CreateUserDto to AppUser on {nameof(CreateUser)}");
            return ApiResult<UserDto>.Failure(new List<string> { "Something Went Wrong!" }, 500);
        }

        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        if (!result.Succeeded)
        {
            return ApiResult<UserDto>.Failure(result.Errors.Select(x => x.Description).ToList(), 400);
        }

        return ApiResult<UserDto>.Success(_mapper.Map<UserDto>(user));

    }

    public async Task<ApiResult<TokenDto>> LoginUser(LoginDto loginDto)
    {
        var data = !string.IsNullOrEmpty(loginDto.UserName) ? loginDto.UserName : loginDto.Email;

        if (string.IsNullOrEmpty(data))
        {
            return ApiResult<TokenDto>.Failure(new List<string> { "Username And Emaila are Empty" }, 401);
        }

        bool isValid = await this.validateUser(loginDto);

        if (!isValid)
        {
            return ApiResult<TokenDto>.Failure(new List<string> { "User Not Found!" }, 404);
        }

        var mapped = _mapper.Map<AppUser>(loginDto);

        var token = _jwtHandler.CreateToken(mapped);

        return ApiResult<TokenDto>.Success(token.Result);

    }


    public async Task<ApiResult<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user != null)
        {
            var userDto = _mapper.Map<UserDto>(user);

            return ApiResult<UserDto>.Success(userDto);
        }

        return ApiResult<UserDto>.Failure(new List<string> { "User not found." }, 404);
    }


    /// <summary>
    /// Changes the password for a user with provided token
    /// </summary>
    public async Task<ApiResult<string>> ChangePasswordAsync(Guid userId, UserChangePasswordDto model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return ApiResult<string>.Failure(new List<string> { "User not found." }, 404);
        }

        if (userId != user.Id)
        {
            return ApiResult<string>.Failure(new List<string> { "User Info is Not Same as Entered Id" }, 400);
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (!result.Succeeded)
        {
            return ApiResult<string>.Failure(result.Errors.Select(e => e.Description).ToList(), 400);
        }

        return ApiResult<string>.Success("Password Has Been Changed!");
    }

    /// <summary>
    /// Validates the password reset token for a user.
    /// </summary>
    public async Task<ApiResult<string>> ValidateToken(UserVerifyTokenDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            _logger.LogCritical($"User Not Found in {nameof(ValidateToken)} with {model.Email}");

            return ApiResult<string>.Failure(new List<string> { "User Not Found" }, 404);
        }
        bool result = await _userManager.VerifyUserTokenAsync(
                          user,
                          _userManager.Options.Tokens.PasswordResetTokenProvider,
                          "ResetPassword",
                          model.Token);

        if (result)
        {
            return ApiResult<string>.Success("Code is Accepted");
        }

        return ApiResult<string>.Failure(new List<string> { "You're Token is WronG!" }, 400);
    }

    /// <summary>
    /// Initiates the process for a user to reset their password.
    /// </summary>
    public async Task<ApiResult<string>> ForgetPassword(UserBase model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            return ApiResult<string>.Failure(new List<string> { "User Not Found" }, 404);
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // We need Add Email Service for This Section

        return ApiResult<string>.Success(token); // It's Temporary!
    }

    public async Task<ApiResult<UserDto>> EditUserDetails(Guid userId, EditUserDto model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return ApiResult<UserDto>.Failure(new List<string> { "User not found." }, 404);
        }

        // Update the properties that are allowed to change
        if (!string.IsNullOrWhiteSpace(model.UserName))
        {
            if (await IsUsernameFree(model.UserName))
            {
                user.UserName = model.UserName;
            }
            else
            {
                return ApiResult<UserDto>.Failure(new List<string>{"Username is Not Available!"}, 400);
            }
        }

        if (!string.IsNullOrWhiteSpace(model.FirstName))
        {
            user.FirstName = model.FirstName;
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                return ApiResult<UserDto>.Failure(new List<string> { "Password is Wrong" }, 400);
            }
            var res = _userManager.ChangePasswordAsync(user, user.PasswordHash, model.Password);

            if (!res.IsCompletedSuccessfully)
            {
                _logger.LogCritical($"Problem for Changing password of user {user.Id} , {res.Result.Errors}");

                return ApiResult<UserDto>.Failure(new List<string> { "Something Went Wrong!" }, 500);
            }
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return ApiResult<UserDto>.Failure(result.Errors.Select(e => e.Description).ToList(), 400);
        }

        var userDto = _mapper.Map<UserDto>(user);

        return ApiResult<UserDto>.Success(userDto);
    }


    public async Task<ApiResult<UserDto>> UserFullDetails()
    {
        var userId = _authService.GetUserIdFromClaims();

        if (userId is null || string.IsNullOrEmpty(userId.ToString()))
        {
            return ApiResult<UserDto>.Failure(new List<string> { "User not found." }, 404);
        }
        var user = await _userManager.FindByIdAsync(userId.ToString()!);

        if (user != null)
        {
            var userDto = _mapper.Map<UserDto>(user);

            return ApiResult<UserDto>.Success(userDto);
        }

        return ApiResult<UserDto>.Failure(new List<string> { "User not found." }, 404);
    }

    public async Task<bool> IsUsernameFree(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return true;
        }

        return false;
    }
    private async Task<bool> validateUser(LoginDto appUserLogin)
    {
        AppUser? appUser;
        if (!string.IsNullOrEmpty(appUserLogin.UserName))
        {
            appUser = await _userManager.FindByNameAsync(appUserLogin.UserName);
        }
        else if (!string.IsNullOrEmpty(appUserLogin.Email))
        {
            appUser = await _userManager.FindByEmailAsync(appUserLogin.Email);
        }
        else
        {
            return false;
        }

        if (appUser != null)
        {
            if (await _userManager.CheckPasswordAsync(appUser, appUserLogin.Password))
            {
                return true;
            }

            return false;
        }
        return false;
    }
}