using HRMSAPI.DTOs;
using HRMSAPI.Models.Entities;

namespace HRMSAPI.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user, List<string> roles, List<string> permissions, List<Guid> organizationIds);
    string GenerateRefreshToken();
}

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RefreshTokenAsync(TokenRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<bool> LogoutAsync(string refreshToken);
}

public interface IOrganizationService
{
    Task<OrganizationDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<OrganizationDto>> GetAllAsync();
    Task<OrganizationDto> CreateAsync(CreateOrganizationRequest request, Guid userId);
    Task<bool> UpdateAsync(UpdateOrganizationRequest request, Guid userId);
    Task<bool> DeleteAsync(Guid id);
}