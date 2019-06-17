namespace WebApi.Application
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public interface IUserUpdate<T>
    {
        Task<T> Execute(T t);
    }


    public class UserUpdate : UserService, IUserUpdate<UserDto>
    {
        public UserUpdate(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(UserDto userDto)
        {
            var userToUpdate = _mapper.Map<User>(userDto);

            try
            {
                var user = await _userRepository.Update(userToUpdate);
                return _mapper.Map<UserDto>(user);
            }
            catch (InvalidOperationException inv)
            {
                throw inv;
            }
        }
    }
}