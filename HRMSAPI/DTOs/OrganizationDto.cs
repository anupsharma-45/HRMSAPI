using System.ComponentModel.DataAnnotations;

namespace HRMSAPI.DTOs;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? GSTIN { get; set; }
    public bool IsActive { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? StateCode { get; set; }
    public string? ZipCode { get; set; }
    public string? CountryCode { get; set; }
    public string? TimeZone { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateOrganizationRequest
{
    [Required, MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? GSTIN { get; set; }

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
}

public class UpdateOrganizationRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? GSTIN { get; set; }

    public bool IsActive { get; set; }

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
}