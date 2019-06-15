namespace WebApi.Application
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.DTO;

    public class UserDelete : UserService, IUserDelete<UserDto>
    {
        public UserDelete(IUserRepository userRepository) : base(userRepository)
        {
        }

        public async Task<bool> Execute(UserDto user)
        {
            try
            {
                await _userRepository.Delete(user.MapFromDto());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
    }
}