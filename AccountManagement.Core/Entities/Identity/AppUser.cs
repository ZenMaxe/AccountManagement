using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity; // For Asp Net 3.0

namespace AccountManagement.Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>
{
    [StringLength(50)]
    [Required]
    public required string FirstName { get; set; }

    [StringLength(60)]
    public string? LastName { get; set; } = null;

    public DateTimeOffset? CreatedDateTime { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Returns the firstname or username for this user
    /// </summary>
    public override string ToString()
        => UserName ?? FirstName; // Email is Not Null
}