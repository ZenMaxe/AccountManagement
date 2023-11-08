using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class UserBase
{
    [EmailAddress]
    public string Email { get; set; }
}