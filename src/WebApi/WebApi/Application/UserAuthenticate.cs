namespace WebApi.Application
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public interface IUserAuthenticate
    {
        Task<UserDto> Authenticate(string username, string password);
    }

    public class UserAuthenticate : IUserAuthenticate
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserAuthenticate(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Authenticate(string username, string password)
        {
            var user = (await _userRepository.Get(x => x.Username.Equals(username))).FirstOrDefault();

            // return null if user not found
            if (user == null || !user.IsPassword(password))
                return null;

            return _mapper.Map<UserDto>(user);
        }
    }
}