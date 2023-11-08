using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class UserChangePasswordDto : UserBase
{
    [Required]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string Token { get; set; }
}