using System.Collections.Generic;
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
            throw new Exception();
//            return this.UserDbContext.User.FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            throw new Exception();
//            return this.UserDbContext.User.FirstOrDefault(u => u.Id == id);
        }

        public void Update(User user)
        {
            throw new Exception();
//            return this.UserDbContext.User.FirstOrDefault(u => u.Id == id);
        }
        
        public void Delete(User user)
        {
            throw new Exception();
//            return this.UserDbContext.User.FirstOrDefault(u => u.Id == id);
        }
    }
}