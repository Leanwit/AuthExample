namespace WebApi.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public sealed class UserRepository : IUserRepository
    {
        public UserRepository(UserDbContext userDbContext)
        {
            UserDbContext = userDbContext;
        }

        private UserDbContext UserDbContext { get; }

        public IEnumerable<User> GetAll()
        {
            return UserDbContext.User;
        }

        public async Task<User> GetById(string id, Func<IQueryable<User>, IQueryable<User>> func = null)
        {
            if (func != null)
                return await func(UserDbContext.User).SingleOrDefaultAsync(o => o.Id.Equals(id));
            return await UserDbContext.User.FindAsync(id);
        }

        public async Task<IQueryable<User>> Get(Expression<Func<User, bool>> where, Func<IQueryable<User>, IQueryable<User>> func = null)
        {
            if (func != null)
                return await Task.FromResult(func(UserDbContext.User.Where(where)));
            return await Task.FromResult(UserDbContext.User.Where(where));
        }


        public async Task<User> Add(User user)
        {
            try
            {
                if (UserDbContext.User.Any(u => u.Id == user.Id || u.Username == user.Username))
                    throw new InvalidOperationException("Id or Username duplicated");

                UserDbContext.User.Add(user);
                await UserDbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<User> Update(User entity)
        {
            try
            {
                var user = UserDbContext.User.First(u => u.Id == entity.Id);

                if (user == null) throw new InvalidOperationException();

                UserDbContext.Entry(user).State = EntityState.Modified;
                await UserDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var exception = e;
                throw e;
            }

            return entity;
        }

        public async Task Delete(string id)
        {
            try
            {
                var user = UserDbContext.User.First(u => u.Id == id);

                if (user == null) throw new InvalidOperationException();
                //this.UserDbContext.Entry(entity).State = EntityState.Deleted;
                UserDbContext.User.Remove(user);
                await UserDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var exception = e;
                throw e;
            }
        }
    }
}