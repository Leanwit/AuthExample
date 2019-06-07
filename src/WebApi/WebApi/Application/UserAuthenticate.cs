namespace WebApi.Application
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.DTO;

    public interface IUserAuthenticate
    {
        Task<UserDto> Authenticate(string username, string password);
    }
    
    public class UserAuthenticate : IUserAuthenticate
    {
        
        private IUserRepository _userRepository;

        public UserAuthenticate(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            
            // return null if user not found
            if (user == null || !user.IsPassword(password))
                return null;

            return user.MapToDto();
        }
    }
}