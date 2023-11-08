using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.DTOs.AppUserDTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}