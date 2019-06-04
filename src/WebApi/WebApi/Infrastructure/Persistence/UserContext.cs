namespace WebApi.Infrastructure.Persistence
{
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserDto> User { get; set; }
    }
}