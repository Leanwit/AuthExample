namespace WebApi.Infraestructure.Persistence
{
    using System;
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
                if (context.User.Any())
                {
                    return;
                }

                context.User.AddRange(
                    new User()
                    {
                        Id = 1,
                        Username = "leanwitzke",
                        Password = "asd1"
                    },
                    new User()
                    {
                        Id = 2,
                        Username = "witzkito",
                        Password = "asd2"
                    });


                context.SaveChanges();
            }
        }
    }
}