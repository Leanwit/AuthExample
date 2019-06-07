namespace WebApi.Test.Domain
{
    using WebApi.Domain;
    using Xunit;

    public class UserTest
    {
        [Theory]
        [InlineData(1, "leandro", "pass")]
        public void New_Is_Assigned_Value(long id, string username, string password)
        {
            var user = new User(id, username, password);
            Assert.NotNull(user);
            Assert.True(user.Id == id);
            Assert.Equal(user.Username, username);
            Assert.Equal(user.Password, password);
        }

        [Theory]
        [InlineData(1, "leandro", "pass")]
        public void Is_Password_Same_Value(long id, string username, string password)
        {
            var user = new User(id, username, password);
            Assert.NotNull(user);
            Assert.True(user.IsPassword(password));
        }

        [Theory]
        [InlineData(1, "leandro", "pass")]
        public void Is_Password_Distinct_Value(long id, string username, string password)
        {
            var user = new User(id, username, password);
            Assert.NotNull(user);
            Assert.False(user.IsPassword("other password"));
        }
    }
}