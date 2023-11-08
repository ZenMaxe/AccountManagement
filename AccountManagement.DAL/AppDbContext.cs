using AccountManagement.Domain.Entities.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.DAL;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>,Guid>, IDataProtectionKeyContext
{

    #region DbSets

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } // For Data Protection on Deploy

    #endregion

    #region Constructor

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}