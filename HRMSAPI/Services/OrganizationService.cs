using AutoMapper;
using HRMSAPI.DTOs;
using HRMSAPI.Interfaces;
using HRMSAPI.Models.Entities;

namespace HRMSAPI.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrganizationDto?> GetByIdAsync(Guid id)
    {
        var organization = await _unitOfWork.Organizations.GetByIdAsync(id);
        if (organization == null || organization.IsDeleted)
            return null;

        return _mapper.Map<OrganizationDto>(organization);
    }

    public async Task<IEnumerable<OrganizationDto>> GetAllAsync()
    {
        var organizations = await _unitOfWork.Organizations.FindAsync(o => !o.IsDeleted);
        return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
    }

    public async Task<OrganizationDto> CreateAsync(CreateOrganizationRequest request, Guid userId)
    {
        var organization = _mapper.Map<Organization>(request);
        organization.CreatedBy = userId;
        organization.CreatedAt = DateTime.UtcNow;
        organization.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Organizations.AddAsync(organization);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<OrganizationDto>(organization);
    }

    public async Task<bool> UpdateAsync(UpdateOrganizationRequest request, Guid userId)
    {
        var organization = await _unitOfWork.Organizations.GetByIdAsync(request.Id);
        if (organization == null || organization.IsDeleted)
            return false;

        _mapper.Map(request, organization);
        organization.UpdatedBy = userId;
        organization.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Organizations.Update(organization);
        return await _unitOfWork.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var organization = await _unitOfWork.Organizations.GetByIdAsync(id);
        if (organization == null || organization.IsDeleted)
            return false;

        organization.IsDeleted = true;
        organization.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Organizations.Update(organization);
        return await _unitOfWork.CompleteAsync() > 0;
    }
}
