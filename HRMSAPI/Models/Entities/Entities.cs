using System.ComponentModel.DataAnnotations;

namespace HRMSAPI.Models.Entities;

public class User : BaseEntity
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }

    // Navigation Properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

public class Role : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    // Navigation Properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class UserRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}

public class Organization : BaseEntity
{
    [Required, MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? GSTIN { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    [MaxLength(50)]
    public string? Address1 { get; set; }
    [MaxLength(50)]
    public string? Address2 { get; set; }
    [MaxLength(50)]
    public string? City { get; set; }
    [MaxLength(10)]
    public string? StateCode { get; set; }
    [MaxLength(25)]
    public string? ZipCode { get; set; }
    [MaxLength(25)]
    public string? CountryCode { get; set; }
    [MaxLength(25)]
    public string? TimeZone { get; set; }

    // Navigation Properties
    public virtual ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
}

public class UserOrganization
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Organization Organization { get; set; } = null!;
}

public class Permission : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Module { get; set; } = string.Empty;

    // Navigation Properties
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class RolePermission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }

    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
}

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? IPAddress { get; set; }
}