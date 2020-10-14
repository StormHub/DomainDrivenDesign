using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Infrastructure.Tests.Repository.EFCoreUnitOfWorkTests
{
    public sealed class TestEntity : DomainEntity
    {
        public Guid ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        public TestEntity()
        {
            Id = new Random().Next();
            Name = "Test";
        }

        public TestEntity(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void DomainMethod()
        {
            DomainEvents.Add(new TestDomainEvent());
            DomainEvents.Add(new AnotherTestDomainEvent());
        }
    }
}
