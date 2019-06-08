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
        Task<User> GetById(long id, Func<IQueryable<User>, IQueryable<User>> func = null);
        Task<User> Update(User entity);
        Task Delete(User entity);
    }
}