namespace WebApi.Application
{
    using Domain.DTO;
    using System.Threading.Tasks;

    interface ICreator
    {
        Task<UserDto> Create(UserDto user);
    }
    
    public class UserCreator : ICreator
    {
        public Task<UserDto> Create(UserDto user)
        {
            throw new System.NotImplementedException();
        }
    }
}