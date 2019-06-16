namespace WebApi.Test.Domain
{
    using Database;
    using WebApi.Domain;
    using Xunit;

    public class UserTest
    {
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