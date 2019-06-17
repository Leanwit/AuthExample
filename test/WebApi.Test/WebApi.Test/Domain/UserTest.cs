namespace WebApi.Test.Domain
{
    using System.IO;
    using System.Linq;
    using Database;
    using WebApi.Domain;
    using Xunit;

    public class UserTest
    {
        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.PageOne)]
        public void Set_Roles(string role)
        {
            var user = new User(UserSeed.Id, UserSeed.Username, UserSeed.Password);
            user.Roles.Add(role);
            Assert.True(user.Id == UserSeed.Id);
            Assert.True(user.Roles.Count() == 1);
            Assert.True(user.Roles.Any(r => r.Equals(role)));
        }

        [Fact]
        public void Is_Password_Distinct_Value()
        {
            var user = new User(UserSeed.Id, UserSeed.Username, UserSeed.Password);
            Assert.NotNull(user);
            Assert.False(user.IsPassword("other password"));
        }

        [Fact]
        public void Is_Password_Same_Value()
        {
            var user = new User(UserSeed.Id, UserSeed.Username, UserSeed.Password);
            Assert.NotNull(user);
            Assert.True(user.IsPassword(UserSeed.Password));
        }

        [Fact]
        public void New_Incorrect_Id()
        {
            Assert.Throws<InvalidDataException>(() => new User("id incorrect", UserSeed.Username, UserSeed.Password));
        }

        [Fact]
        public void New_Is_Assigned_Value()
        {
            var user = new User(UserSeed.Id, UserSeed.Username, UserSeed.Password);
            Assert.NotNull(user);
            Assert.True(user.Id == UserSeed.Id);
            Assert.Equal(user.Username, UserSeed.Username);
            Assert.Equal(user.Password, UserSeed.Password);
        }
    }
}