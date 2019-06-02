using System.Collections.Generic;
using WebApi.Domain;

namespace WebApi.Test.Database
{
    internal static class UserSeed
    {
        public static List<User> CreateUsers(int count = 1)
        {
            var userList = new List<User>();
            for (int i = 0; i < count; i++)
            {
                userList.Add(new User()
                {
                    Id = i+1,
                    Email = "leanwitzke@gmail.com",
                    Password = "asd1"
                });
            }

            return userList;
        }

        public static User CreateSpecificUser(long id, string email = "default@gmail.com", string password = "asd")
        {
            return new User()
            {
                Id = id, Email = email, Password = password
            };
        }
    }
}