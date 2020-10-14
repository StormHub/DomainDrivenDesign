using System.Threading;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : IQueryResponse
    {
        Task<TResponse> Handle(TQuery query, CancellationToken token);
    }
}