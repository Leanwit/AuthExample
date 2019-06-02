namespace WebApi.Infraestructure.Persistence
{
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}