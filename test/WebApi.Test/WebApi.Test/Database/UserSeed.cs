namespace WebApi.Test.Database
{
    using System.Collections.Generic;
    using Domain;

    internal static class UserSeed
    {
        public static List<User> CreateUsers(int count = 1)
        {
            var userList = new List<User>();
            for (int i = 0; i < count; i++)
            {
                userList.Add(new User()
                {
                    Id = i + 1,
                    Username = "leanwitzke",
                    Password = "asd1"
                });
            }

            return userList;
        }

        public static User CreateSpecificUser(long id, string email = "default", string password = "asd")
        {
            return new User()
            {
                Id = id, Username = email, Password = password
            };
        }
    }
}