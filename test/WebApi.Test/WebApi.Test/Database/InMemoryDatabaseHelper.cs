namespace WebApi.Test.Database
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using WebApi.Domain;
    using WebApi.Infrastructure.Persistence;

    internal static class InMemoryDatabaseHelper
    {
        internal static void Save(List<User> users, UserDbContext userDbContext)
        {
            userDbContext.AddRange(users);
            userDbContext.SaveChanges();
        }

        internal static UserDbContext CreateContext(string databaseName)
        {
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder.UseInMemoryDatabase(databaseName);
            return new UserDbContext(builder.Options);
        }
    }
}