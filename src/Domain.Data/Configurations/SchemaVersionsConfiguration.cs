using Domain.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Data.Configurations
{
    sealed class SchemaVersionsConfiguration : IEntityTypeConfiguration<SchemaVersions>
    {
        public void Configure(EntityTypeBuilder<SchemaVersions> builder)
        {
            builder.HasNoKey();
        }
    }
}
