namespace WebApi.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Database;
    using Helper;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Application;
    using WebApi.Controllers;
    using WebApi.Domain;
    using WebApi.Domain.DTO;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class UserControllerTest
    {
        private UserController GetControllerInstance(UserDbContext context)
        {
            var mapper = Services.CreateAutoMapperObjectUsingUserProfile();
            var repository = new UserRepository(context);
            var userFinder = new UserFind(repository, mapper);
            var userDelete = new UserDelete(repository, mapper);
            var userCreate = new UserCreate(repository, mapper);
            return new UserController(userFinder, userDelete, userCreate);
        }

        [Theory]
        [InlineData(UserSeed.Username, "")]
        [InlineData("", UserSeed.Password)]
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

        [Fact]
        public async Task Delete_Existing_User()
        {
            var dbName = nameof(Delete_Existing_User);
            var user = UserSeed.CreateUserTest();
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(new List<User> {user}, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var controller = GetControllerInstance(context);

                var result = await controller.Delete(user.Id);
                Assert.IsType<OkResult>(result);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                Assert.False(context.User.Any(u => u.Id == user.Id));
            }
        }

        [Fact]
        public async Task Delete_Existing_User_Multiple_Users()
        {
            var users = UserSeed.CreateUsers(10);
            var userGenerated = UserSeed.CreateUserTest();
            users.Add(userGenerated);
            var dbName = nameof(Delete_Existing_User_Multiple_Users);

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(users, context);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                var controller = GetControllerInstance(context);

                var result = await controller.Delete(userGenerated.Id);
                Assert.IsType<OkResult>(result);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                Assert.False(context.User.Any(u => u.Id == userGenerated.Id));
                Assert.True(context.User.Count() == users.Count() - 1);
            }
        }

        [Fact]
        public async Task Delete_No_Existing_User()
        {
            var dbName = nameof(Delete_No_Existing_User);
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);

                var controller = GetControllerInstance(context);
                Assert.IsType<NotFoundObjectResult>(await controller.Delete(Guid.NewGuid().ToString()));
            }
        }

        [Fact]
        public async Task Get_Id_201_Code()
        {
            var id = Guid.NewGuid().ToString();
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Get_Id_201_Code)))
            {
                InMemoryDatabaseHelper.Save(new List<User> {UserSeed.CreateSpecificUser(id)}, context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.Get(id);

                // Assert
                Assert.IsType<ActionResult<UserFindDto>>(actionResult);
                Assert.IsType<UserFindDto>(actionResult.Value);
                Assert.True(actionResult.Value.Id == id);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Get_Id_201_Code)))
            {
                Assert.True(context.User.Any(u => u.Id == id));
            }
        }

        [Fact]
        public async Task Get_Id_404_Code()
        {
            var id = Guid.NewGuid().ToString();
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Get_Id_404_Code)))
            {
                InMemoryDatabaseHelper.Save(UserSeed.CreateUsers(), context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.Get(id);

                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Get_Id_404_Code)))
            {
                Assert.False(context.User.Any(u => u.Id == id));
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
                Assert.IsType<ActionResult<IEnumerable<UserFindDto>>>(actionResult);
                Assert.True(actionResult.Value.Any());
            }
        }

        [Fact]
        public async Task GetByUsername_201_Code()
        {
            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(GetByUsername_201_Code)))
            {
                InMemoryDatabaseHelper.Save(new List<User> {UserSeed.CreateSpecificUser(UserSeed.Id)}, context);
                var controller = GetControllerInstance(context);

                var actionResult = await controller.GetByUsername(UserSeed.Username);

                Assert.IsType<ActionResult<UserFindDto>>(actionResult);
                Assert.Equal(actionResult.Value.Username, UserSeed.Username);
                Assert.True(actionResult.Value.Id == UserSeed.Id);
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

        [Fact]
        public async Task Post_Create_User_Conflict()
        {
            var user = UserSeed.CreateUserTest();
            var dbName = nameof(Post_Create_User_Conflict);
            using (var context = InMemoryDatabaseHelper.CreateContext(dbName))
            {
                InMemoryDatabaseHelper.Save(new List<User> {user}, context);

                var controller = GetControllerInstance(context);

                var dto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password
                };

                var result = await controller.Post(dto);
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        [Fact]
        public async Task Post_Create_User_Success()
        {
            var userGenerated = UserSeed.CreateUserTest();

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Post_Create_User_Success)))
            {
                var controller = GetControllerInstance(context);
                var dto = new UserDto
                {
                    Id = userGenerated.Id,
                    Username = userGenerated.Username,
                    Password = userGenerated.Password
                };

                var result = await controller.Post(dto);
                var dtoSuccess = result.Value;

                Assert.IsType<ActionResult<UserDto>>(result);
                Assert.IsType<UserDto>(dtoSuccess);
            }

            using (var context = InMemoryDatabaseHelper.CreateContext(nameof(Post_Create_User_Success)))
            {
                var user = context.User.FirstOrDefault(u => u.Id == userGenerated.Id);
                Assert.NotNull(user);
                Assert.True(user.Username == userGenerated.Username);
                Assert.True(user.Password == userGenerated.Password);
            }
        }
    }
}