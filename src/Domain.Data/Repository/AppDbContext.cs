using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Domain.Data.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException($"{nameof(optionsBuilder)} is not configured");
            }

            optionsBuilder.ConfigureWarnings(warnings =>
            {
                warnings
#if DEBUG
                    .Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning)
#endif
                    .Default(WarningBehavior.Throw);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
