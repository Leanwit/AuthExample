namespace WebApi.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public class UserDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UserDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<UserDbContext>>()))
            {
                if (context.User.Any()) return;

                var user = new User(Guid.NewGuid().ToString(), "leanwitzke", "asd1");
                user.Roles = new List<Role> {Role.Admin};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), "witzkito", "asd2");
                user.Roles = new List<Role> {Role.PageOne, Role.PageTwo};
                context.User.Add(user);

                context.SaveChanges();
            }
        }
    }
}