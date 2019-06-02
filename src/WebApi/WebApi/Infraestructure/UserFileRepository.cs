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
    }
}