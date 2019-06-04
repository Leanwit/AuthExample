using System.Collections.Generic;

namespace WebApi.Domain
{
    public interface IUserRepository
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetById(long id);
        UserDto GetByUsername(string username);

        UserDto Add(UserDto userDto);
        UserDto Update(UserDto userDto);
        void Delete(UserDto userDto);
    }
}