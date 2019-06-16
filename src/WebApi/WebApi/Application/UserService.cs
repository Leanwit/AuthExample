namespace WebApi.Application
{
    using AutoMapper;
    using Domain;

    public abstract class UserService
    {
        protected readonly IMapper _mapper;
        protected readonly IUserRepository _userRepository;

        protected UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
    }
}