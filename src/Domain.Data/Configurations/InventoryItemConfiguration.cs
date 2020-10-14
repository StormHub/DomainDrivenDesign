using Inventory.Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Data.Configurations
{
    sealed class InventoryItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item), "Inventory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExternalId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)");
            builder.Property(x => x.DeliveryFee).HasColumnType("decimal(8, 2)");
        }
    }
}
