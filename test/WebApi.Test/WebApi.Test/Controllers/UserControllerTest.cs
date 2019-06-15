namespace WebApi.Test.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Application;
    using WebApi.Controllers;
    using WebApi.Domain;
    using WebApi.Domain.DTO;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserControllerTest
    {
        [Theory]
        [InlineData(10, "defaultuser")]
        public async Task GetByUsername_201_Code(long id, string username)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_201_Code)))
            {
                InMemoryDatabaseHelper.Save(new List<User> {UserSeed.CreateSpecificUser(id, username)}, context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.GetByUsername(username);

                Assert.IsType<ActionResult<UserDto>>(actionResult);
                Assert.Equal(actionResult.Value.Username, username);
                Assert.True(actionResult.Value.Id == id);
            }
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(15, 10)]
        public async Task Get_Id_404_Code(long id, int usersCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext($"{nameof(Get_Id_404_Code)}_{id}_{usersCount}"))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(usersCount), context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.Get(id);

                // Assert
                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 10)]
        public async Task Get_Id_201_Code(long id, int usersCount)
        {
            using (var context = InMemoryDatabaseHelper.CreateContext($"{nameof(Get_Id_201_Code)}_{id}_{usersCount}"))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(usersCount), context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.Get(id);

                // Assert
                Assert.IsType<ActionResult<UserDto>>(actionResult);
                Assert.IsType<UserDto>(actionResult.Value);
                Assert.True(actionResult.Value.Id == id);
            }
        }

        [Theory]
        [InlineData(1, "leanwitzke", "password")]
        [InlineData(2, "defaultuser", "%$qdqw&&132")]
        public async Task Post_Create_User_Success(long id, string username, string password)
        {
            var dbName = $"{nameof(Post_Create_User_Success)}_{username}";
            long idAux;
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var controller = GetControllerInstance(context);

                var dto = new UserDto
                {
                    Id = id,
                    Username = username,
                    Password = password
                };

                var result = await controller.Post(dto);
                var dtoSuccess = result.Value;
                idAux = dtoSuccess.Id;

                Assert.IsType<ActionResult<UserDto>>(result);
                Assert.IsType<UserDto>(dtoSuccess);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var user = context.User.FirstOrDefault(u => u.Id == idAux);
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
            var dbName = $"{nameof(Post_Create_User_Invalid_Model)}_{username}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var controller = GetControllerInstance(context);

                var dto = new UserDto
                {
                    Username = username,
                    Password = password
                };

                var result = await controller.Post(dto);
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        [Theory]
        [InlineData(1, "leanwitzke", "password")]
        public async Task Post_Create_User_Conflict(long id, string username, string password)
        {
            var dbName = $"{nameof(Post_Create_User_Conflict)}_{username}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(new List<User> {UserSeed.CreateSpecificUser(id, username)}, context);

                var controller = GetControllerInstance(context);

                var dto = new UserDto
                {
                    Username = username,
                    Password = password
                };

                var result = await controller.Post(dto);
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        [Theory]
        [InlineData(1, "leanwitzke", "password")]
        public async Task Delete_Existing_User(long id, string username, string password)
        {
            var dbName = $"{nameof(Delete_Existing_User)}_{username}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(new List<User> {UserSeed.CreateSpecificUser(id, username)}, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var controller = GetControllerInstance(context);

                var result = await controller.Delete(id);
                Assert.IsType<OkResult>(result);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                Assert.False(context.User.Any(u => u.Id == id));
            }
        }

        [Theory]
        [InlineData(1, 10)]
        public async Task Delete_Existing_User_Multiple_Users(long id, int userCount)
        {
            var dbName = $"{nameof(Delete_Existing_User_Multiple_Users)}_{id}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(userCount), context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var controller = GetControllerInstance(context);

                var result = await controller.Delete(id);
                Assert.IsType<OkResult>(result);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                Assert.False(context.User.Any(u => u.Id == id));
                Assert.True(context.User.Count() == userCount - 1);
            }
        }

        [Theory]
        [InlineData(10)]
        public async Task Delete_No_Existing_User(long id)
        {
            var dbName = $"{nameof(Delete_No_Existing_User)}_{id}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);

                var controller = GetControllerInstance(context);
                Assert.IsType<NotFoundObjectResult>(await controller.Delete(id));
            }
        }

        [Theory]
        [InlineData(-10)]
        public async Task Delete_Check_Bad_Parameter(long id)
        {
            var dbName = $"{nameof(Delete_No_Existing_User)}_{id}";
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);

                var controller = GetControllerInstance(context);
                Assert.IsType<BadRequestObjectResult>(await controller.Delete(id));
            }
        }

        private UserController GetControllerInstance(UserDbContext context)
        {
            var repository = new UserRepository(context);
            var userFinder = new UserFind(repository);
            var userDelete = new UserDelete(repository);
            var userCreate = new UserCreate(repository);
            return new UserController(userFinder, userDelete, userCreate);
        }

        [Fact]
        public async Task Get_Id_400_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Get_Id_400_Code)))
            {
                var controller = GetControllerInstance(context);

                var actionResult = await controller.Get(0);

                // Assert
                Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            }
        }

        [Fact]
        public void GetAll_ReturnValues()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetAll_ReturnValues)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                var controller = GetControllerInstance(context);

                var actionResult = controller.GetAll();

                // Assert
                Assert.IsType<ActionResult<IEnumerable<UserDto>>>(actionResult);
                Assert.True(actionResult.Value.Any());
            }
        }

        [Fact]
        public async Task GetByUsername_400_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_400_Code)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.GetByUsername(string.Empty);

                // Assert
                Assert.IsType<BadRequestObjectResult>(actionResult.Result);
                Assert.IsNotType<OkResult>(actionResult);
            }
        }

        [Fact]
        public async Task GetByUsername_404_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_404_Code)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.GetByUsername("user");

                // Assert
                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
                Assert.IsNotType<OkResult>(actionResult);
            }
        }
    }
}