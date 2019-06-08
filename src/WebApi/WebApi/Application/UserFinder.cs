namespace WebApi.Application
{
    using System.Collections.Generic;
    using Domain;
    using Domain.DTO;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserFinder
    {
        IEnumerable<UserDto> GetAll();
        Task<UserDto> GetByUsername(string username);
    }

    public class UserFinder : IUserFinder
    {
        private IUserRepository _userRepository;

        public UserFinder(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = this._userRepository.GetAll();

            return UserMapper.MapToDto(users);
        }

        public async Task<UserDto> GetByUsername(string username)
        {
            var user = (await _userRepository.Get(x => x.Username.Equals(username))).FirstOrDefault();
            return UserMapper.MapToDto(user);
        }
    }
}