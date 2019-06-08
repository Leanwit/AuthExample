namespace WebApi.Test.Infrastructure
{
    using Database;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Domain;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserFileRepositoryTest
    {
        private string CreateDatabaseName(string name)
        {
            return $"{nameof(UserFileRepositoryTest)}_{name}";
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
                UserFileRepository repository = new UserFileRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.True(Enumerable.Any(userList));
            }
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(0)]
        public void GetAll_Is_Correct_Count(int userCount)
        {
            string dbName = $"{nameof(GetAll_Is_Correct_Count)}_{userCount}";
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.Equal(userCount, Enumerable.Count(userList));
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 10)]
        [InlineData(10, 10)]
        public async Task GetById_Is_Exist_Value(long id, int userCount)
        {
            string dbName = $"{nameof(GetById_Is_Exist_Value)}_{id}_{userCount}";

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var user = await repository.GetById(id);

                Assert.NotNull(user);
                Assert.True(user.Id == id);
            }
        }

        [Theory]
        [InlineData(3, 1)]
        public async Task GetById_Is_Not_Exist_Value(long id, int userCount)
        {
            string dbName = $"{nameof(GetById_Is_Not_Exist_Value)}_{id}";

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(dbName)))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var user = await repository.GetById(id);

                Assert.Null(user);
            }
        }

        [Theory]
        [InlineData(5, 1, "defaultuser")]
        public void Add_Is_Not_Exist_Value(long id, int userCount, string email)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Add_Is_Not_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
                UserFileRepository repository = new UserFileRepository(context);
                repository.Add(UserSeed.CreateSpecificUser(id, email));
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Add_Is_Not_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.True(Enumerable.Any(userList, u => u.Id == id));
                Assert.True(Enumerable.Any(userList, u => u.Username == email));
                Assert.True(Enumerable.Count(userList) == userCount + 1);
            }
        }

        [Theory]
        [InlineData(5, 10, "defaultuser")]
        public void Add_Duplicate_Id(long id, int userCount, string email)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Add_Duplicate_Id))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
                UserFileRepository repository = new UserFileRepository(context);
                Assert.Throws<InvalidOperationException>(() => repository.Add(UserSeed.CreateSpecificUser(id, email)));
            }
        }

        [Theory]
        [InlineData(5, 10, "defaultuser")]
        public async Task Update_Is_Exist_Value(long id, int userCount, string email)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var user = await repository.GetById(id);
                user.Username = email;
                await repository.Update(user);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.True(Enumerable.Any<User>(userList, u => u.Username == email));
                Assert.True(Enumerable.Count<User>(userList) == userCount);
            }
        }

        [Theory]
        [InlineData(5, 10)]
        public async Task Update_Is_Not_Exist_Value(long id, int userCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Not_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Update_Is_Not_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var user = await repository.GetById(id);
                user.Id = userCount * 10;
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.Update(user));
            }
        }

        [Theory]
        [InlineData(5, 10)]
        public async Task Delete_Is_Exist_Value(long id, int userCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                await repository.Delete(await repository.GetById(id));
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var userList = repository.GetAll();

                Assert.NotNull(userList);
                Assert.False(Enumerable.Any<User>(userList, u => u.Id == id));
                Assert.True(Enumerable.Count<User>(userList) == userCount - 1);
            }
        }

        [Theory]
        [InlineData(5, 10)]
        public async Task Delete_Is_Not_Exist_Value(long id, int userCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Not_Exist_Value))))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(CreateDatabaseName(nameof(Delete_Is_Not_Exist_Value))))
            {
                UserFileRepository repository = new UserFileRepository(context);
                var user = await repository.GetById(id);

                user.Id = userCount * 10;
                await Assert.ThrowsAsync<InvalidOperationException>(() => repository.Delete(user));
            }
        }
    }
}