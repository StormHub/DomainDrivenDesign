using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.Messaging;

namespace Domain.Infrastructure
{
    public interface IMediator
    {
        Task Command<TCommand>(
            TCommand command,
            CancellationToken cancellationToken)
            where TCommand : class, ICommand;

        Task Raise<TDomainEvent>(
            TDomainEvent domainEvent,
            CancellationToken cancellationToken)
            where TDomainEvent : class, IDomainEvent;

        Task<TQueryResponse> Query<TQuery, TQueryResponse>(
            TQuery query,
            CancellationToken cancellationToken)
            where TQuery : IQuery<TQueryResponse>
            where TQueryResponse : class, IQueryResponse;
    }
}