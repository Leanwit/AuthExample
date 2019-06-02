﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Infraestructure
{
    using System;
    using System.IO;
    using WebApi.Domain;

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

        public User GetById(long id)
        {
            return this.UserDbContext.User.FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            try
            {
                this.UserDbContext.User.Add(user);
                this.UserDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(User user)
        {
            try
            {
                this.UserDbContext.User.Update(user);
                this.UserDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public void Delete(User user)
        {
            try
            {
                this.UserDbContext.User.Remove(user);
                this.UserDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}