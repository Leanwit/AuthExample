using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Domain;

namespace WebApi.Infraestructure
{
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
                        Email = "leanwitzke@gmail.com",
                        Password = "asd1"
                    },
                    new User()
                    {
                        Id = 2,
                        Email = "witzkito@gmail.com",
                        Password = "asd2"
                    });


                context.SaveChanges();
            }
        }
    }
}