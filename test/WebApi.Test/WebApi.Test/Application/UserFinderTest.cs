namespace WebApi.Test.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using WebApi.Application;
    using WebApi.Domain;
    using Xunit;

    public class UserFinderTest
    {
        [Fact]
        public void GetAll_Return_Value()
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers()).GetInMemoryUserRepository();

            var userFinder = new UserFinder(sut);

            var users = userFinder.GetAll();
            Assert.NotNull(users);
            Assert.True(users.Any());
        }

        [Fact]
        public void GetAll_No_Return_Value()
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers(0)).GetInMemoryUserRepository();

            var userFinder = new UserFinder(sut);

            var users = userFinder.GetAll();
            Assert.NotNull(users);
            Assert.False(users.Any());

        }

        [Theory]
        [InlineData(10, "defaultuser")]
        public void GetByUsername_Return_Value(long id, string username)
        {
            var sut = new InMemoryDatabase(new List<User>()
            {
                UserSeed.CreateSpecificUser(id, username)
            }).GetInMemoryUserRepository();

            var userFinder = new UserFinder(sut);

            var user = userFinder.GetByUsername(username);
            Assert.NotNull(user);
            Assert.Equal(user.Username, username);
        }

        [Theory]
        [InlineData(30, "defaultuser")]
        public void GetByUsername_No_Return_Value(long id, string username)
        {
            var sut = new InMemoryDatabase(UserSeed.CreateUsers()).GetInMemoryUserRepository();

            var userFinder = new UserFinder(sut);

            var user = userFinder.GetByUsername(username);
            Assert.Null(user);
        }
    }
}