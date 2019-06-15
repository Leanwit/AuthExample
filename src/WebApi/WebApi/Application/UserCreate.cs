namespace WebApi.Application
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.DTO;

    public class UserCreate : UserService, IUserCreate<UserDto>
    {
        public UserCreate(IUserRepository userRepository) : base(userRepository)
        {
        }

        public async Task<UserDto> Execute(UserDto userDto)
        {
            var user = userDto.MapFromDto();

            try
            {
                return (await _userRepository.Add(user))?.MapToDto();
            }
            catch (InvalidOperationException inv)
            {
                throw inv;
            }
        }
    }
}