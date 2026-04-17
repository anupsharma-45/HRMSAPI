using HRMSAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<UserOrganization> UserOrganizations { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).HasColumnName("Password");
            entity.Property(e => e.Phone).HasColumnName("PhoneNumber");
        });

        // Organization Configuration
        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
        });

        // UserRole Configuration
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        });

        // UserOrganization Configuration
        modelBuilder.Entity<UserOrganization>(entity =>
        {
            entity.HasOne(uo => uo.User)
                .WithMany(u => u.UserOrganizations)
                .HasForeignKey(uo => uo.UserId);

            entity.HasOne(uo => uo.Organization)
                .WithMany(o => o.UserOrganizations)
                .HasForeignKey(uo => uo.OrganizationId);
        });

        // RolePermission Configuration
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            entity.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        });

        // RefreshToken Configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);
        });
    }
}