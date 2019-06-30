namespace WebApi.Test.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Helper;
    using Helper.Database;
    using Microsoft.EntityFrameworkCore;
    using WebApi.Application;
    using WebApi.Domain;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserFinderTest
    {
        private UserFind CreateUserFindObject(UserDbContext context)
        {
            var repository = new UserRepository(context);
            return new UserFind(repository, Services.CreateAutoMapperObjectUsingUserProfile());
        }

        [Fact]
        public void GetAll_No_Return_Value()
        {
            // Arrange
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_No_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(0), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_No_Return_Value)))
            {
                // Act
                var userFinder = CreateUserFindObject(context);
                var users = userFinder.GetAll();

                // Assert
                Assert.NotNull(users);
                Assert.False(users.Any());
            }
        }

        [Fact]
        public void GetAll_Return_Value()
        {
            // Arrange
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_Return_Value)))
            {
                // Act
                var userFinder = CreateUserFindObject(context);
                var users = userFinder.GetAll();

                // Assert
                Assert.NotNull(users);
                Assert.True(users.Any());
            }
        }

        [Fact]
        public async Task GetByUsername_No_Return_Value()
        {
            // Arrange
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_No_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_No_Return_Value)))
            {
                // Act
                var userFinder = CreateUserFindObject(context);
                var user = await userFinder.GetByUsername(userGenerated.Username);

                // Assert
                Assert.Null(user);
                Assert.DoesNotContain(userGenerated.Username, context.User.Select(u => u.Username));
            }
        }

        [Fact]
        public async Task GetByUsername_Return_Value()
        {
            // Arrange
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_Return_Value)))
            {
                InMemoryDatabaseHelper.Save(new List<User> {userGenerated}, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_Return_Value)))
            {
                // Act
                var userFinder = CreateUserFindObject(context);
                var user = await userFinder.GetByUsername(userGenerated.Username);
                
                // Assert
                Assert.NotNull(user);
                Assert.Equal(user.Username, userGenerated.Username);
                Assert.Equal(user.Id, userGenerated.Id);
            }
        }
    }
}