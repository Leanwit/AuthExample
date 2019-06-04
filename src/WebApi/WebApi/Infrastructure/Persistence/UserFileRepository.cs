namespace WebApi.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class UserFileRepository : IUserRepository
    {
        private UserDbContext UserDbContext { get; set; }

        public UserFileRepository(UserDbContext userDbContext)
        {
            this.UserDbContext = userDbContext;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return this.UserDbContext.User;
        }

        public UserDto GetById(long id)
        {
            return this.UserDbContext.User.FirstOrDefault(u => u.Id == id);
        }
        
        public UserDto GetByUsername(string username)
        {
            return this.UserDbContext.User.FirstOrDefault(u => u.Username == username);
        }

        public UserDto Add(UserDto userDto)
        {
            try
            {
                this.UserDbContext.User.Add(userDto);
                this.UserDbContext.SaveChanges();
                return userDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UserDto Update(UserDto userDto)
        {
            try
            {
                this.UserDbContext.User.Update(userDto);
                this.UserDbContext.SaveChanges();
                return userDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public void Delete(UserDto userDto)
        {
            try
            {
                this.UserDbContext.User.Remove(userDto);
                this.UserDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}