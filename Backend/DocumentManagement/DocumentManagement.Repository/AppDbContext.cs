using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository
{
  public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.HasDefaultSchema("dbo");

      modelBuilder.Entity<Account>(entity => { entity.HasKey(e => e.Id); });
    }
  }
}