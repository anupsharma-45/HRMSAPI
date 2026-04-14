using HRMSAPI.DTOs;
using HRMSAPI.Interfaces;
using HRMSAPI.Models.Entities;
using BCrypt.Net;

namespace HRMSAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _config;

    public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _config = config;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetUserWithDetailsAsync(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) || !user.IsActive || user.IsDeleted)
        {
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .Distinct()
            .ToList();
        var organizationIds = user.UserOrganizations.Select(uo => uo.OrganizationId).ToList();

        var accessToken = _jwtService.GenerateAccessToken(user, roles, permissions, organizationIds);
        var refreshTokenString = _jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenString,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        
        user.LastLoginAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        
        await _unitOfWork.CompleteAsync();

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Roles = roles,
            Permissions = permissions,
            OrganizationIds = organizationIds,
            Token = accessToken,
            RefreshToken = refreshTokenString,
            RefreshTokenExpiry = refreshToken.ExpiryDate
        };
    }

    public async Task<AuthResponse?> RefreshTokenAsync(TokenRequest request)
    {
        var refreshToken = (await _unitOfWork.RefreshTokens.FindAsync(t => t.Token == request.RefreshToken)).FirstOrDefault();

        if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiryDate < DateTime.UtcNow)
        {
            return null;
        }

        var user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId);
        if (user == null) return null;

        refreshToken.IsRevoked = true;
        
        var userWithDetails = await _unitOfWork.Users.GetUserWithDetailsAsync(user.Email);
        var roles = userWithDetails!.UserRoles.Select(ur => ur.Role.Name).ToList();
        var permissions = userWithDetails.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .Distinct()
            .ToList();
        var organizationIds = userWithDetails.UserOrganizations.Select(uo => uo.OrganizationId).ToList();

        var newAccessToken = _jwtService.GenerateAccessToken(user, roles, permissions, organizationIds);
        var newRefreshTokenString = _jwtService.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshTokenString,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
        await _unitOfWork.CompleteAsync();

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Roles = roles,
            Permissions = permissions,
            OrganizationIds = organizationIds,
            Token = newAccessToken,
            RefreshToken = newRefreshTokenString,
            RefreshTokenExpiry = newRefreshToken.ExpiryDate
        };
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (await _unitOfWork.Users.GetByEmailAsync(request.Email) != null)
        {
            return false;
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _unitOfWork.Users.AddAsync(user);
        return await _unitOfWork.CompleteAsync() > 0;
    }
}