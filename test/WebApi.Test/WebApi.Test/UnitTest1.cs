


using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using WebApi.Domain;

namespace WebApi.Test
{
    using Infraestructure;
    using Xunit;
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IUserRepository sut = GetInMemoryUserRepository();

            
            Assert.NotNull(sut.GetAll());
        }
        
        
        private IUserRepository GetInMemoryUserRepository()
        {
            DbContextOptions<UserDbContext> options;
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder.UseInMemoryDatabase();
            options = builder.Options;
            UserDbContext userDbContext = new UserDbContext(options);
            userDbContext.Database.EnsureDeleted();
            userDbContext.Database.EnsureCreated();

            userDbContext.User.Add(
                new User()
                {
                    Id = 1,
                    Email = "leanwitzke@gmail.com",
                    Password = "asd1"
                });
            userDbContext.SaveChanges();

            return new UserFileRepository(userDbContext);
        }
    }
}