namespace WebApi.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(e => e.Roles)
                .HasConversion(
                    v => v.ToString(),
                    v => (List<Role>) Enum.Parse(typeof(Role), v.ToString()));
        }
    }
}