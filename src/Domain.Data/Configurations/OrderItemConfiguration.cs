using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities.Items;

namespace Domain.Data.Configurations
{
    sealed class OrderItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item), "Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExternalId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)");
            builder.Property(x => x.DeliveryFee).HasColumnType("decimal(8, 2)");
        }
    }
}
