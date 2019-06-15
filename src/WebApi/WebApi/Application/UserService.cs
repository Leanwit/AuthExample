namespace WebApi.Application
{
    using Domain;

    public abstract class UserService
    {
        protected IUserRepository _userRepository;

        protected UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}