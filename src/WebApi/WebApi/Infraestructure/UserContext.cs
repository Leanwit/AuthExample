using Microsoft.EntityFrameworkCore;
using WebApi.Domain;

namespace WebApi.Infraestructure
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}