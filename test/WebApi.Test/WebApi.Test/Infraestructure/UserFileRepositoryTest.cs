namespace WebApi.Test.Infraestructure
{
    using System;
    using System.Linq;
    using Database;
    using Xunit;

    public class UserFileRepositoryTest
    {
        [Fact]
        public void GetAll_Return_Value()
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers()).GetInMemoryUserRepository();
            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.True(userList.Any());
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
            Assert.Equal(userCount, userList.Count());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 15)]
        public void GetById_Is_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var user = sut.GetById(id);

            Assert.NotNull(user);
            Assert.True(user.Id == id);
        }

        [Theory]
        [InlineData(5, 1)]
        [InlineData(50, 15)]
        public void GetById_Is_Not_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var user = sut.GetById(id);

            Assert.Null(user);
        }

        [Theory]
        [InlineData(100, 1, "defaultuser")]
        [InlineData(155, 100, "otheruser")]
        public void Add_Is_Exist_Value(long id, int userCount, string email)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            sut.Add(UserSeed.CreateSpecificUser(id, email));

            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.True(userList.Any(u => u.Id == id));
            Assert.True(userList.Any(u => u.Username == email));
            Assert.True(userList.Count() == userCount + 1);
        }

        [Theory]
        [InlineData(5, 10, "defaultuser")]
        [InlineData(10, 50, "otheruser")]
        public void Add_Duplicate_Id(long id, int userCount, string email)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            Assert.Throws<InvalidOperationException>(() => sut.Add(UserSeed.CreateSpecificUser(id, email)));
        }

        [Theory]
        [InlineData(1, 5, "defaultuser")]
        [InlineData(80, 100, "otheruser")]
        public void Update_Is_Exist_Value(long id, int userCount, string email)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();

            var user = sut.GetById(id);
            user.Username = email;
            sut.Update(user);

            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.True(userList.Any(u => u.Username == email));
            Assert.True(userList.Count() == userCount);
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(80, 100)]
        public void Update_Is_Not_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();

            var user = sut.GetById(id);
            user.Id = userCount * 2;
            Assert.Throws<InvalidOperationException>(() => sut.Update(user));
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(80, 100)]
        public void Delete_Is_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();

            sut.Delete(sut.GetById(id));

            var userList = sut.GetAll();

            Assert.NotNull(userList);
            Assert.False(userList.Any(u => u.Id == id));
            Assert.True(userList.Count() == userCount - 1);
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(80, 100)]
        public void Delete_Is_Not_Exist_Value(long id, int userCount)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(userCount)).GetInMemoryUserRepository();
            var user = sut.GetById(id);
            user.Id = userCount * 2;
            Assert.Throws<InvalidOperationException>(() => sut.Delete(user));
        }
    }
}