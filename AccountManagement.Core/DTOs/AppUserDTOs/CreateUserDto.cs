using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class CreateUserDto : UserBase
{
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username May Between 3-100 Characters")]
    public required string UserName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string FirstName { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public required string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; } = string.Empty;
}