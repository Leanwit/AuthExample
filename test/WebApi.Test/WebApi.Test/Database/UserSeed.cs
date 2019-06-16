namespace WebApi.Test.Database
{
    using System;
    using System.Collections.Generic;
    using Faker;
    using WebApi.Domain;

    internal static class UserSeed
    {
        public const string Id = "11e93406-b12c-4ebd-8820-e5201d6f2fa5";
        public const string Username = "default";
        public const string Password = "1234";

        public static User CreateUserTest()
        {
            return new User(Guid.NewGuid().ToString(), Name.First(), new Guid().ToString());
        }

        public static List<User> CreateUsers(int count = 1)
        {
            var userList = new List<User>();
            for (var i = 0; i < count; i++) userList.Add(new User(Guid.NewGuid().ToString(), Name.Last(), Guid.NewGuid().ToString()));

            return userList;
        }

        public static User CreateSpecificUser(string id, string username = Username, string password = Password)
        {
            return new User(id, username, password);
        }
    }
}