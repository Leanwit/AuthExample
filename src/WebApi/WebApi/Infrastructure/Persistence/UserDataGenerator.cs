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

                var user = new User("fbb27b43-179b-4898-b9af-cdda8ef4a503", "admin", "admin");
                user.Roles = new List<string> {Role.Admin};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), "witzkito", "asd2");
                user.Roles = new List<string> {Role.PageOne, Role.PageTwo};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), "pageone", "pageone");
                user.Roles = new List<string> {Role.PageOne};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), "pagetwo", "pagetwo");
                user.Roles = new List<string> {Role.PageTwo};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), "pagethree", "pagethree");
                user.Roles = new List<string> {Role.PageThree};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), "norole", "norole");
                user.Roles = new List<string>();
                context.User.Add(user);
                context.SaveChanges();
            }
        }
    }
}