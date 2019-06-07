using System.Collections.Generic;

namespace WebApi.Domain
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(long id);
        User GetByUsername(string username);

        User Add(User userDto);
        User Update(User userDto);
        void Delete(User userDto);
    }
}