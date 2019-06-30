namespace WebApi.Test.Application
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Helper;
    using Helper.Database;
    using WebApi.Application;
    using WebApi.Domain.DTO;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserDeleteTest
    {
        private UserDelete CreateUserDeleteObject(UserDbContext context)
        {
            var repository = new UserRepository(context);
            return new UserDelete(repository, Services.CreateAutoMapperObjectUsingUserProfile());
        }

        [Fact]
        private async Task Execute_Does_Not_Exist()
        {
            var userCount = 10;
            // Arrange
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Execute_Does_Not_Exist)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Execute_Does_Not_Exist)))
            {
                var userDelete = CreateUserDeleteObject(context);
                var userDto = new UserDto();

                // Act && Assert
                await Assert.ThrowsAsync<Exception>(async () => await userDelete.Execute(userDto));
            }
        }

        [Fact]
        private async Task Execute_Success()
        {
            var userCount = 10;
            // Arrange
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Execute_Success)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Execute_Success)))
            {
                var user = context.User.FirstOrDefault();
                var userDelete = CreateUserDeleteObject(context);
                var userDto = new UserDto {Id = user.Id, Username = user.Username, Password = user.Password};

                // Act
                var result = await userDelete.Execute(userDto);

                // Assert
                Assert.DoesNotContain(user.Id, context.User.Select(u => u.Id));
                Assert.Equal(context.User.Count(), userCount - 1);
                Assert.IsType<UserDto>(result);
            }
        }
    }
}