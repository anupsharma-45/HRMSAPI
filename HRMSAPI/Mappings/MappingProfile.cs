using AutoMapper;
using HRMSAPI.DTOs;
using HRMSAPI.Models.Entities;

namespace HRMSAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<User, AuthResponse>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}