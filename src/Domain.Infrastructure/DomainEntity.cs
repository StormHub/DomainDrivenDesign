using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace Domain.Infrastructure
{
    public abstract class DomainEntity : IDomainEntity
    {
        [Key]
        public int Id { get; protected set; }

        [NotMapped]
        public DomainEventsCollection DomainEvents { get; } = new DomainEventsCollection();
    }
}