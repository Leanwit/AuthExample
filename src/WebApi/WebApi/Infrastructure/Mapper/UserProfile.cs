namespace WebApi.Infrastructure.Mapper
{
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserFindDto>().ReverseMap();
            CreateMap<UserDto, UserFindDto>().ReverseMap();
        }
    }
}