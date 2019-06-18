namespace WebApi.Application
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public interface IUserCreate<T>
    {
        Task<T> Execute(T user);
    }

    public class UserCreate : UserService, IUserCreate<UserDto>
    {
        public UserCreate(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(UserDto userDto)
        {
            var userToCreate = _mapper.Map<User>(userDto);

            try
            {
                var user = await _userRepository.Add(userToCreate);
                return _mapper.Map<UserDto>(user);
            }
            catch (InvalidOperationException inv)
            {
                throw inv;
            }
        }
    }
}