using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class UserVerifyTokenDto : UserBase
{
    [Required]
    public string Token { get; set; }
}