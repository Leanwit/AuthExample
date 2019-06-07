namespace WebApi.Test.Database
{
    using System.Collections.Generic;
    using WebApi.Domain;

    internal static class UserSeed
    {
        public static List<User> CreateUsers(int count = 1)
        {
            var userList = new List<User>();
            for (int i = 0; i < count; i++)
            {
                userList.Add(new User(i + 1, "leanwitzke", "asd1"));
            }

            return userList;
        }

        public static User CreateSpecificUser(long id, string username = "default", string password = "asd")
        {
            return new User(id, username, password);
        }
    }
}