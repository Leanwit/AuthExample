namespace WebApi.Test.Controllers
{
    using Database;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Application;
    using WebApi.Controllers;
    using WebApi.Domain;
    using WebApi.Domain.DTO;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserControllerTest
    {
        [Fact]
        public async Task GetByUsername_404_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_404_Code)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                ActionResult<UserDto> actionResult = await controller.GetByUsername("user");

                // Assert
                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
                Assert.IsNotType<OkResult>(actionResult);
            }
        }

        [Theory]
        [InlineData(10, "defaultuser")]
        public async Task GetByUsername_201_Code(long id, string username)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_201_Code)))
            {
                InMemoryDatabaseHelper.Save(new List<User>() {UserSeed.CreateSpecificUser(id, username)}, context);
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                ActionResult<UserDto> actionResult = await controller.GetByUsername(username);

                Assert.IsType<ActionResult<UserDto>>(actionResult);
                Assert.True(actionResult.Value.Username.Equals(username));
                Assert.True(actionResult.Value.Id == id);
            }
        }

        [Fact]
        public async Task GetByUsername_400_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_400_Code)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                ActionResult<UserDto> actionResult = await controller.GetByUsername(string.Empty);

                // Assert
                Assert.IsType<BadRequestObjectResult>(actionResult.Result);
                Assert.IsNotType<OkResult>(actionResult);
            }
        }

        [Fact]
        public void GetAll_ReturnValues()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_ReturnValues)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                var actionResult = controller.GetAll();

                // Assert
                Assert.IsType<ActionResult<IEnumerable<UserDto>>>(actionResult);
                Assert.True(actionResult.Value.Any());
            }
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(15, 10)]
        public async Task GetById_404_Code(long id, int usersCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext($"{nameof(GetById_404_Code)}_{id}_{usersCount}"))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(usersCount), context);
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                ActionResult<UserDto> actionResult = await controller.GetById(id);

                // Assert
                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 10)]
        public async Task GetById_201_Code(long id, int usersCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext($"{nameof(GetById_201_Code)}_{id}_{usersCount}"))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(usersCount), context);
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                ActionResult<UserDto> actionResult = await controller.GetById(id);

                // Assert
                Assert.IsType<ActionResult<UserDto>>(actionResult);
                Assert.IsType<UserDto>(actionResult.Value);
                Assert.True(actionResult.Value.Id == id);
            }
        }

        [Fact]
        public async Task GetById_400_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetById_400_Code)))
            {
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);

                var controller = new UserController(finder);
                ActionResult<UserDto> actionResult = await controller.GetById(0);

                // Assert
                Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            }
        }
    }
}