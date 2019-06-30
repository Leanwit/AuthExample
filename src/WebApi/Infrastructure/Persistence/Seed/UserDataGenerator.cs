namespace WebApi.Infrastructure.Persistence.Seed
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public static class UserDataGenerator
    {
        public const string PageOne = "pageone";
        public const string PageTwo = "pagetwo";
        public const string PageThree = "pagethree";
        public const string NoRole = "norole";
        public const string Admin = "admin";
        public const string PageOneTwo = "pageonetwo";
        public const string PageThreeAdmin = "pagethreeadmin";

        public const string GuidAdmin = "11e93406-b12c-4ebd-8820-e5201d6f2fa5";
        public const string GuidUserPageOne = "11e93406-b12c-4ebd-8820-e5201d6f2fa6";

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UserDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<UserDbContext>>()))
            {
                if (context.User.Any()) return;

                var user = new User(GuidAdmin, Admin, Admin);
                user.Roles = new List<string> {Role.Admin};
                context.User.Add(user);

                user = new User(GuidUserPageOne, PageOne, PageOne);
                user.Roles = new List<string> {Role.PageOne};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), PageTwo, PageTwo);
                user.Roles = new List<string> {Role.PageTwo};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), PageThree, PageThree);
                user.Roles = new List<string> {Role.PageThree};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), NoRole, NoRole);
                user.Roles = new List<string>();
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), PageOneTwo, PageOneTwo);
                user.Roles = new List<string> {Role.PageOne, Role.PageTwo};
                context.User.Add(user);

                user = new User(Guid.NewGuid().ToString(), PageThreeAdmin, PageThreeAdmin);
                user.Roles = new List<string> {Role.PageThree, Role.Admin};
                context.User.Add(user);
                context.SaveChanges();
            }
        }
    }
}