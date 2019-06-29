namespace WebApi.Application
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public interface IUserDelete<T>
    {
        Task<T> Execute(T id);
    }

    public class UserDelete : UserService, IUserDelete<UserDto>
    {
        public UserDelete(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(UserDto userDto)
        {
            try
            {
                return _mapper.Map<UserDto>(await _userRepository.Delete(userDto.Id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
    }
}