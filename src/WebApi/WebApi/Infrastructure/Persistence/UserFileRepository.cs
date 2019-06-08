namespace WebApi.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class UserFileRepository : IUserRepository
    {
        private UserDbContext UserDbContext { get; set; }

        public UserFileRepository(UserDbContext userDbContext)
        {
            this.UserDbContext = userDbContext;
        }

        public IEnumerable<User> GetAll()
        {
            return this.UserDbContext.User;
        }

        public virtual async Task<User> GetById(long id, Func<IQueryable<User>, IQueryable<User>> func = null)
        {
            if (func != null)
            {
                return await func(this.UserDbContext.User).SingleOrDefaultAsync(o => o.Id.Equals(id));
            }
            else
            {
                return await this.UserDbContext.User.FindAsync(id);
            }
        }
        public virtual async Task<IQueryable<User>> Get(Expression<Func<User, bool>> where, Func<IQueryable<User>, IQueryable<User>> func = null)
        {
            if (func != null)
            {
                return await Task.FromResult<IQueryable<User>>(func(this.UserDbContext.User.Where(where)));
            }
            else
            {
                return await Task.FromResult<IQueryable<User>>(this.UserDbContext.User.Where(where));
            }
        }


        public User Add(User user)
        {
            try
            {
                this.UserDbContext.User.Add(user);
                this.UserDbContext.SaveChanges();
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public virtual async Task<User> Update(User entity)
        {
            try
            {
                this.UserDbContext.Entry(entity).State = EntityState.Modified;
                await this.UserDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var exception = e;
                throw e;
            }

            return entity;
        }

        public virtual async Task Delete(User entity)
        {
            try
            {
                this.UserDbContext.User.Remove(entity);
                await this.UserDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var exception = e;
                throw e;
            }
        }

    }
}