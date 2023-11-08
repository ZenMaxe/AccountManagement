using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class LoginDto
{

    public string UserName { get; set; }

    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MaxLength(100)]
    public string Password { get; set; }
}