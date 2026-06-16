using AutoMapper;
using MarketHub.Application.Models.Authentication;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegisterationDto, User>();
    }
}