namespace WebApi.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        IEnumerable<User> GetAll();

        Task<IQueryable<User>> Get(Expression<Func<User, bool>> where, Func<IQueryable<User>, IQueryable<User>> func = null);

        Task<User> GetById(string id, Func<IQueryable<User>, IQueryable<User>> func = null);
        Task<User> Update(User entity);
        Task<User> Delete(string id);

        Task<User> Add(User user);
    }
}