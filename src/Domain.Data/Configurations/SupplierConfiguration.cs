using Inventory.Domain.Entities.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Data.Configurations
{
    sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable(nameof(Supplier), "Inventory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExternalId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
        }
    }
}
