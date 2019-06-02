using System.Collections.Generic;

namespace WebApi.Domain
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
    }
}