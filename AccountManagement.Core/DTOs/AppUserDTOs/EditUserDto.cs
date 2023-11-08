using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class EditUserDto
{

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username May Between 3-100 Characters")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string FirstName { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string OldPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}