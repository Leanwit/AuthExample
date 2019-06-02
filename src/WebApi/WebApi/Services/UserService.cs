namespace WebApi.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;

    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            //TODO: Get by Username
            var user = _userRepository.GetAll().SingleOrDefault(x => x.Username == username);

            // return null if user not found
            if (user == null || !user.IsPassword(password))
                return null;

            // TODO: return UserDto without password
            return user;
        }
    }
}