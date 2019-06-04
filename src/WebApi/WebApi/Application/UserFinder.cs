namespace WebApi.Application
{
    using System.Collections.Generic;
    using Domain;
    using Domain.DTO;
    using UserDto = Domain.DTO.UserDto;


    public interface IUserFinder
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetByUsername(string username);
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

        public UserDto GetByUsername(string username)
        {
            var user = this._userRepository.GetByUsername(username);
            return UserMapper.MapToDto(user);
        }
    }
}