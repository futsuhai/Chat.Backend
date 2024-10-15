using AutoMapper;
using Chat_Backend.Models.Backend;
using Chat_Backend.Models.Frontend;

namespace Chat_Backend.Mappers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<AccountModel, Account>()
            .ForMember(dest => dest.Salt, opt => opt.Ignore())
            .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
            .ReverseMap();
    }
}