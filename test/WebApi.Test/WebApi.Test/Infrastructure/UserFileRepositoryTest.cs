namespace WebApi.Test.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Helper.Database;
    using WebApi.Domain;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserFileRepositoryTest
    {
        private string CreateDatabaseName(string name)
        {
            return $"{nameof(UserFileRepositoryTest)}_{name}";
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(0)]
        public void GetAll_Is_Correct_Count(int userCount)
        {
            var dbName = $"{nameof(GetAll_Is_Correct_Count)}_{userCount}";
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.Equal(userCount, userList.Count());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public async Task GetById_Is_Exist_Value(int userCount)
        {
            var dbName = $"{nameof(GetById_Is_Exist_Value)}_{userCount}";
            var userGenerated = UserSeed.CreateUserTest();
            var users = UserSeed.CreateUsers(userCount);
            users.Add(userGenerated);

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                InMemoryDatabaseHelper.Save(users, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetById(userGenerated.Id);

                Assert.NotNull(user);
                Assert.True(user.Id == userGenerated.Id);
                Assert.Equal(user.Password, userGenerated.Password);
            }
        }

        [Theory]
        [InlineData(10)]
        public async Task Delete_Is_Exist_Value(int userCount)
        {
            var userGenerated = UserSeed.CreateUserTest();
            var users = UserSeed.CreateUsers(userCount);
            users.Add(userGenerated);

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(users, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Exist_Value))))
            {
                var repository = new UserRepository(context);
                await repository.Delete(userGenerated.Id);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Exist_Value))))
            {
                Assert.NotNull(context.User);
                Assert.False(context.User.Any(u => u.Id == userGenerated.Id));
                Assert.True(context.User.Count() == userCount);
            }
        }

        [Fact]
        public async Task Add_Duplicate_Id()
        {
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Add_Duplicate_Id))))
            {
                InMemoryDatabaseHelper.Save(new List<User> {userGenerated}, context);
                var repository = new UserRepository(context);
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.Add(userGenerated));
            }
        }

        [Fact]
        public void Add_New_Value()
        {
            var userCount = 10;
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Add_New_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
                var repository = new UserRepository(context);
                repository.Add(userGenerated);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Add_New_Value))))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.True(userList.Any(u => u.Id == userGenerated.Id));
                Assert.True(userList.Any(u => u.Username == userGenerated.Username));
                Assert.True(userList.Count() == userCount + 1);
            }
        }

        [Fact]
        public async Task Delete_Is_Not_Exist_Value()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Not_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Not_Exist_Value))))
            {
                var repository = new UserRepository(context);
                await Assert.ThrowsAsync<InvalidOperationException>(() => repository.Delete(UserSeed.CreateUserTest().Id));
            }
        }

        [Fact]
        public void GetAll_Return_Value()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(GetAll_Return_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(GetAll_Return_Value))))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.True(userList.Any());
            }
        }

        [Fact]
        public async Task GetById_Is_Not_Exist_Value()
        {
            var dbName = nameof(GetById_Is_Not_Exist_Value);
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetById(Guid.NewGuid().ToString());

                Assert.Null(user);
            }
        }

        [Fact]
        public async Task Update_Is_Exist_Value()
        {
            var userGenerated = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(new List<User> {userGenerated}, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Exist_Value))))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetById(userGenerated.Id);
                user.Username = UserSeed.Username;
                await repository.Update(user);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Exist_Value))))
            {
                var repository = new UserRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.True(userList.Any(u => u.Username == UserSeed.Username));
            }
        }

        [Fact]
        public async Task Update_Is_Not_Exist_Value()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Not_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Not_Exist_Value))))
            {
                var repository = new UserRepository(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.Update(UserSeed.CreateUserTest()));
            }
        }
    }
}