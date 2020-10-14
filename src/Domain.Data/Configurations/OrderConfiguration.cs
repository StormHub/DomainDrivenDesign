using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities.Orders;

namespace Domain.Data.Configurations
{
    sealed class OrderConfiguration : IEntityTypeConfiguration<Order>, IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order), "Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ExternalId).IsRequired();
            builder.Property(x => x.DeliveryFee).HasColumnType("decimal(18, 2)");
            builder.Property(x => x.Priority).IsRequired();
        }

        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable(nameof(OrderItem), "Orders");
            builder.HasKey(x => new { x.LineNumber, x.OrderId });

            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(8, 2)");
        }
    }
}
