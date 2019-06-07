namespace WebApi.Test.Infrastructure
{
    using System;
    using System.Linq;
    using Database;
    using WebApi.Domain;
    using Xunit;

    public class UserFileRepositoryTest
    {
        [Fact]
        public void GetAll_Return_Value()
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers()).GetInMemoryUserRepository();
            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.True(Enumerable.Any<User>(userList));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(30)]
        [InlineData(0)]
        public void GetAll_Is_Correct_Count(int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.Equal(userCount, Enumerable.Count(userList));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 150)]
        public void GetById_Is_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var user = sut.GetById(id);

            Assert.NotNull(user);
            Assert.True(user.Id == id);
        }

        [Theory]
        [InlineData(3, 1)]
        public void GetById_Is_Not_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var user = sut.GetById(id);

            Assert.Null(user);
        }

        [Theory]
        [InlineData(5, 1, "defaultuser")]
        public void Add_Is_Not_Exist_Value(long id, int userCount, string email)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            sut.Add(UserSeed.CreateSpecificUser(id, email));

            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.True(Enumerable.Any(userList, u => u.Id == id));
            Assert.True(Enumerable.Any(userList, u => u.Username == email));
            Assert.True(Enumerable.Count(userList) == userCount + 1);
        }

        [Theory]
        [InlineData(7, 10, "defaultuser")]
        public void Add_Duplicate_Id(long id, int userCount, string email)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            Assert.Throws<InvalidOperationException>(() => sut.Add(UserSeed.CreateSpecificUser(id, email)));
        }

        [Theory]
        [InlineData(9, 15, "defaultuser")]
        public void Update_Is_Exist_Value(long id, int userCount, string email)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();

            var user = sut.GetById(id);
            user.Username = email;
            sut.Update(user);

            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.True(Enumerable.Any<User>(userList, u => u.Username == email));
            Assert.True(Enumerable.Count<User>(userList) == userCount);
        }

        [Theory]
        [InlineData(6, 10)]
        public void Update_Is_Not_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();

            var user = sut.GetById(id);
            user.Id = userCount * 2;
            Assert.Throws<InvalidOperationException>(() => sut.Update(user));
        }

        [Theory]
        [InlineData(10, 15)]
        public void Delete_Is_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();

            sut.Delete(sut.GetById(id));

            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.False(Enumerable.Any<User>(userList, u => u.Id == id));
            Assert.True(Enumerable.Count<User>(userList) == userCount - 1);
        }

        [Theory]
        [InlineData(15, 25)]
        public void Delete_Is_Not_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var user = sut.GetById(id);
            user.Id = userCount * 2;
            Assert.NotNull(sut);
            Assert.NotNull(user);
            Assert.Throws<InvalidOperationException>(() => sut.Delete(user));
        }
    }
}