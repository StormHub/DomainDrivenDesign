using System.Threading;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Messaging
{
    public interface ICommandHandler<in T>
        where T : ICommand
    {
        Task Handle(T command, CancellationToken token);
    }
}
