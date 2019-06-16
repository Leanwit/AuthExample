namespace WebApi.Application
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public class UserDelete : UserService, IUserDelete<UserDto>
    {
        public UserDelete(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
        }

        public async Task<bool> Execute(string id)
        {
            try
            {
                await _userRepository.Delete(id);
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