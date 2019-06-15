namespace WebApi.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.DTO;

    public class UserFind : UserService, IUserFind<UserDto>
    {
        public UserFind(IUserRepository userRepository) : base(userRepository)
        {
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _userRepository.GetAll();

            return users.MapToDto();
        }

        public async Task<UserDto> GetByUsername(string username)
        {
            var user = (await _userRepository.Get(x => x.Username.Equals(username))).FirstOrDefault();
            return user.MapToDto();
        }

        public async Task<UserDto> GetById(long id)
        {
            var user = (await _userRepository.Get(x => x.Id == id)).FirstOrDefault();
            return user.MapToDto();
        }
    }
}