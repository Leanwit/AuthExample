namespace WebApi.Test.Database
{
    using System.Collections.Generic;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using WebApi.Infraestructure;
    using WebApi.Infraestructure.Persistence;

    public class InMemoryDatabase
    {
        private List<User> Users { get; set; }

        public InMemoryDatabase(List<User> users)
        {
            this.Users = users;
        }

        public UserFileRepository GetInMemoryUserRepository()
        {
            DbContextOptions<UserDbContext> options;
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder.UseInMemoryDatabase();
            options = builder.Options;
            UserDbContext userDbContext = new UserDbContext(options);
            userDbContext.Database.EnsureDeleted();
            userDbContext.Database.EnsureCreated();

            userDbContext.AddRange(this.Users);

            userDbContext.SaveChanges();

            return new UserFileRepository(userDbContext);
        }
    }
}