using System;

namespace Domain.Infrastructure.Messaging
{
    public interface ICommand
    {
        DateTimeOffset OccurredOn { get; set; }
    }
}
