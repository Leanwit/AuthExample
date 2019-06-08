namespace WebApi.Test.Application
{
    using Database;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Application;
    using WebApi.Domain;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserFinderTest
    {
        private DbContextOptions<UserDbContext> CreateDbOptions(string databaseName)
        {
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder.UseInMemoryDatabase(databaseName);
            return builder.Options;
        }

        [Fact]
        public async Task GetAll_Return_Value()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_Return_Value)))
            {
                var repository = new UserFileRepository(context);
                var userFinder = new UserFinder(repository);
                var users = userFinder.GetAll();
                Assert.NotNull(users);
                Assert.True(users.Any());
            }
        }

        [Fact]
        public async Task GetAll_No_Return_Value()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_No_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(0), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_No_Return_Value)))
            {
                var repository = new UserFileRepository(context);
                var userFinder = new UserFinder(repository);

                var users = userFinder.GetAll();
                Assert.NotNull(users);
                Assert.False(users.Any());
            }
        }

        [Theory]
        [InlineData(10, "defaultuser")]
        public async Task GetByUsername_Return_Value(long id, string username)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(new List<User>()
                {
                    UserSeed.CreateSpecificUser(id, username)
                }, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_Return_Value)))
            {
                var repository = new UserFileRepository(context);
                var userFinder = new UserFinder(repository);

                var user = await userFinder.GetByUsername(username);
                Assert.NotNull(user);
                Assert.Equal(user.Username, username);
            }
        }

        [Theory]
        [InlineData(10, "defaultuser")]
        public async Task GetByUsername_No_Return_Value(long id, string username)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_No_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_No_Return_Value)))
            {
                var repository = new UserFileRepository(context);
                var userFinder = new UserFinder(repository);

                var user = await userFinder.GetByUsername(username);
                Assert.Null(user);
            }
        }
    }
}