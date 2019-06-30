namespace WebApi.Infrastructure.Mapper
{
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ForMember(u => u.Password, opt => opt.Ignore());
            CreateMap<UserDto, User>();
            CreateMap<User, UserFindDto>().ReverseMap();
            CreateMap<UserDto, UserFindDto>().ReverseMap();
        }
    }
}