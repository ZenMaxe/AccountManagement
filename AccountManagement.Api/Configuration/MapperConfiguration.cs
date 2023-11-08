using AccountManagement.Domain.DTOs.AppUserDTOs;
using AccountManagement.Domain.Entities.Identity;
using AutoMapper;

namespace AccountManagement.Configuration;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        userModelsMap();
    }

    private void userModelsMap()
    {
        CreateMap<AppUser, CreateUserDto>()
           .ReverseMap();

        CreateMap<AppUser, UserDto>()
           .ReverseMap();
        CreateMap<AppUser, LoginDto>()
           .ReverseMap();
        CreateMap<AppUser, UserBase>()
           .ReverseMap();


    }
}