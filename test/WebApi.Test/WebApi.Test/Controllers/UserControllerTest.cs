namespace WebApi.Test.Controllers
{
    using Database;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
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
                Assert.Equal(actionResult.Value.Username, username);
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
                ActionResult<UserDto> actionResult = await controller.Get(id);

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
                ActionResult<UserDto> actionResult = await controller.Get(id);

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
                ActionResult<UserDto> actionResult = await controller.Get(0);

                // Assert
                Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            }
        }

        [Theory]
        [InlineData("leanwitzke", "password")]
        [InlineData("defaultuser", "%$qdqw&&132")]
        public async Task Post_Create_User_Success(string username, string password)
        {
            string dbName = $"{nameof(Post_Create_User_Success)}_{username}";
            long id = 0;
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);
                var controller = new UserController(finder);

                UserCreateDto dto = new UserCreateDto
                {
                    Username = username,
                    Password = password
                };

                var result = await controller.Post(dto);
                UserDto dtoSuccess = result.Value;
                id = dtoSuccess.Id;
                
                Assert.IsType<ActionResult<UserDto>>(result);
                Assert.IsType<UserDto>(dtoSuccess);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var user = context.User.FirstOrDefault(u => u.Id == id);
                Assert.NotNull(user);
                Assert.True(user.Username == username);
                Assert.True(user.Password == password);
            }
        }

        [Theory]
        [InlineData("leanwitzke", "")]
        [InlineData("", "%$qdqw&&132")]
        public async Task Post_Create_User_Invalid_Model(string username, string password)
        {
            string dbName = $"{nameof(Post_Create_User_Invalid_Model)}_{username}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);
                var controller = new UserController(finder);

                UserCreateDto dto = new UserCreateDto()
                {
                    Username = username,
                    Password = password
                };

                var result = await controller.Post(dto);
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }
        
        [Theory]
        [InlineData(1,"leanwitzke", "password")]
        public async Task Post_Create_User_Conflict(long id,string username, string password)
        {
            string dbName = $"{nameof(Post_Create_User_Conflict)}_{username}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(new List<User>{UserSeed.CreateSpecificUser(id,username)}, context);

                UserFileRepository repository = new UserFileRepository(context);
                UserFinder finder = new UserFinder(repository);
                var controller = new UserController(finder);

                UserCreateDto dto = new UserCreateDto()
                {
                    Username = username,
                    Password = password
                };

                var result = await controller.Post(dto);
                Assert.IsType<ConflictObjectResult>(result.Result);
            }
        }
    }
}