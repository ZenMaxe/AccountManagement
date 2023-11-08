using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AccountsController : ControllerBase
{
    readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService) { _accountService = accountService; }



    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        var result = await _accountService.CreateUser(createUserDto);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _accountService.LoginUser(loginDto);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var result = await _accountService.GetUserByIdAsync(userId);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpPost("change-password/{userId:guid}")]
    public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] UserChangePasswordDto changePasswordDto)
    {
        var result = await _accountService.ChangePasswordAsync(userId, changePasswordDto);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpPost("validate-token")]
    public async Task<IActionResult> ValidateToken([FromBody] UserVerifyTokenDto verifyTokenDto)
    {
        var result = await _accountService.ValidateToken(verifyTokenDto);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] UserBase userBase)
    {
        var result = await _accountService.ForgetPassword(userBase);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpPut("edit-details")]
    public async Task<IActionResult> EditUserDetails(Guid userId, [FromBody] EditUserDto editUserDto)
    {
        var result = await _accountService.EditUserDetails(userId, editUserDto);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }

    [HttpGet("details")]
    public async Task<IActionResult> UserFullDetails()
    {
        var result = await _accountService.UserFullDetails();
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }
}