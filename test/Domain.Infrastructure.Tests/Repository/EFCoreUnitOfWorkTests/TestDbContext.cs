using System;
using Microsoft.EntityFrameworkCore;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException($"{nameof(optionsBuilder)} is not configured");
            }
            // Throw exception when query evaluation is done on client
            optionsBuilder.ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Throw));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestEntity>();
        }
    }
}
