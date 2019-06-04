namespace WebApi.Infrastructure.Persistence
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
                    new UserDto(1, "leanwitzke", "asd1"),
                    new UserDto(2, "witzkito", "asd2")
                );

                context.SaveChanges();
            }
        }
    }
}