using HRMSAPI.DTOs;
using HRMSAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMSAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OrganizationController : BaseController
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var organization = await _organizationService.GetByIdAsync(id);
        if (organization == null)
            return NotFoundResponse("Organization not found.");

        return Success(organization);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var organizations = await _organizationService.GetAllAsync();
        return Success(organizations);
    }

    [HttpPost("create")]
    [Authorize(Policy = "RequireSuperAdmin")]
    public async Task<IActionResult> Create(CreateOrganizationRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized();

        var userId = Guid.Parse(userIdString);
        var organization = await _organizationService.CreateAsync(request, userId);

        return Success(organization, "Organization created successfully.");
    }

    [HttpPut("update")]
    [Authorize(Policy = "RequireSuperAdmin")]
    public async Task<IActionResult> Update(UpdateOrganizationRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized();

        var userId = Guid.Parse(userIdString);
        var result = await _organizationService.UpdateAsync(request, userId);

        if (!result)
            return NotFoundResponse("Organization not found or update failed.");

        return Success(true, "Organization updated successfully.");
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Policy = "RequireSuperAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _organizationService.DeleteAsync(id);

        if (!result)
            return NotFoundResponse("Organization not found or delete failed.");

        return Success(true, "Organization deleted successfully.");
    }
}
