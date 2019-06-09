namespace WebApi.Application
{
    using Domain.DTO;
    using System.Threading.Tasks;

    interface ICreator
    {
        Task<UserDto> Create(UserCreateDto user);
    }
    
    public class UserCreator : ICreator
    {
        public Task<UserDto> Create(UserCreateDto user)
        {
            throw new System.NotImplementedException();
        }
    }
}