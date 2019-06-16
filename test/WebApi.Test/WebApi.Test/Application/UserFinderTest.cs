namespace WebApi.Test.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.EntityFrameworkCore;
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

        private UserFind CreateUserFind(UserDbContext context)
        {
            var repository = new UserRepository(context);
            return new UserFind(repository);
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
                var userFinder = CreateUserFind(context);

                var users = userFinder.GetAll();

                Assert.NotNull(users);
                Assert.False(users.Any());
            }
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
                var userFinder = CreateUserFind(context);

                var users = userFinder.GetAll();

                Assert.NotNull(users);

                Assert.True(users.Any());
            }
        }

        [Fact]
        public async Task GetByUsername_No_Return_Value()
        {
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_No_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_No_Return_Value)))
            {
                var userFinder = CreateUserFind(context);

                var user = await userFinder.GetByUsername(userGenerated.Username);

                Assert.Null(user);
            }
        }

        [Fact]
        public async Task GetByUsername_Return_Value()
        {
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(new List<User> {userGenerated}, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_Return_Value)))
            {
                var userFinder = CreateUserFind(context);

                var user = await userFinder.GetByUsername(userGenerated.Username);
                Assert.NotNull(user);
                Assert.Equal(user.Username, userGenerated.Username);
                Assert.Equal(user.Id, userGenerated.Id);
            }
        }
    }
}