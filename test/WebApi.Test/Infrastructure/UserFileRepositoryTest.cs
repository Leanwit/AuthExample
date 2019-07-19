namespace WebApi.Test.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Helper.Database;
    using WebApi.Domain;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserFileRepositoryTest
    {
        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(0)]
        public void GetAll_Is_Correct_Count(int userCount)
        {
            var dbName = GivenADatabaseContext(userCount);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.Equal(userCount, userList.Count());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public async Task GetById_Is_Exist_Value(int userCount)
        {
            var dbName = Guid.NewGuid().ToString();

            var userGenerated = GivenADatabaseContextWithUserGenerated(userCount, dbName);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetById(userGenerated.Id);

                Assert.True(user.Id == userGenerated.Id);
                Assert.Equal(user.Password, userGenerated.Password);
            }
        }

        [Theory]
        [InlineData(10)]
        public async Task Delete_Is_Exist_Value(int userCount)
        {
            var dbName = Guid.NewGuid().ToString();

            var userGenerated = GivenADatabaseContextWithUserGenerated(userCount, dbName);

            await WhenDeleteAUser(dbName, userGenerated);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                Assert.NotNull(context.User);
                Assert.False(context.User.Any(u => u.Id == userGenerated.Id));
                Assert.True(context.User.Count() == userCount);
            }
        }

        [Theory]
        [InlineData(10)]
        public async Task Add_New_Value(int userCount)
        {
            var dbName = GivenADatabaseContext(userCount);

            var userGenerated = UserSeed.CreateUserTest();

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                await repository.Add(userGenerated);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll().ToList();

                Assert.True(userList.Count() == userCount + 1);
                Assert.Contains(userGenerated.Id, userList.Select(u => u.Id));
            }
        }

        private static async Task<string> WhenUpdateUsername(string dbName, User userGenerated)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetById(userGenerated.Id);
                user.Username = UserSeed.Username;
                await repository.Update(user);

                return UserSeed.Username;
            }
        }

        private User GivenADatabaseContextWithUserGenerated(int userCount, string dbName)
        {
            var userGenerated = UserSeed.CreateUserTest();
            var users = UserSeed.CreateUsers(userCount);
            users.Add(userGenerated);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(users, context);
            }

            return userGenerated;
        }

        private async Task WhenDeleteAUser(string dbName, User userGenerated)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                await repository.Delete(userGenerated.Id);
            }
        }

        private string GivenADatabaseContext(int userCount)
        {
            var dbName = Guid.NewGuid().ToString();
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            return dbName;
        }

        [Fact]
        public async Task Add_Duplicate_Id()
        {
            var dbName = Guid.NewGuid().ToString();

            var userGenerated = GivenADatabaseContextWithUserGenerated(10, dbName);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.Add(userGenerated));
            }
        }

        [Fact]
        public async Task Delete_Is_Not_Exist_Value()
        {
            var dbName = GivenADatabaseContext(1);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                await Assert.ThrowsAsync<InvalidOperationException>(() => repository.Delete(UserSeed.CreateUserTest().Id));
            }
        }

        [Fact]
        public void GetAll_Return_Value()
        {
            var dbName = GivenADatabaseContext(10);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.True(userList.Any());
            }
        }

        [Fact]
        public async Task GetById_Is_Not_Exist_Value()
        {
            var dbName = GivenADatabaseContext(10);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetById(Guid.NewGuid().ToString());

                Assert.Null(user);
            }
        }

        [Fact]
        public async Task Update_Is_Exist_Value()
        {
            var dbName = Guid.NewGuid().ToString();
            var userGenerated = GivenADatabaseContextWithUserGenerated(10, dbName);

            var username = await WhenUpdateUsername(dbName, userGenerated);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.Contains(username, userList.Select(u => u.Username));
            }
        }

        [Fact]
        public async Task Update_Is_Not_Exist_Value()
        {
            var dbName = GivenADatabaseContext(10);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var repository = new UserRepository(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.Update(UserSeed.CreateUserTest()));
            }
        }
    }
}