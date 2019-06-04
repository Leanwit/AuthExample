namespace WebApi.Test.Database
{
    using System.Collections.Generic;
    using WebApi.Domain;

    internal static class UserSeed
    {
        public static List<UserDto> CreateUsers(int count = 1)
        {
            var userList = new List<UserDto>();
            for (int i = 0; i < count; i++)
            {
                userList.Add(new UserDto(i + 1, "leanwitzke", "asd1"));
            }

            return userList;
        }

        public static UserDto CreateSpecificUser(long id, string email = "default", string password = "asd")
        {
            return new UserDto(id, email, password);
        }
    }
}