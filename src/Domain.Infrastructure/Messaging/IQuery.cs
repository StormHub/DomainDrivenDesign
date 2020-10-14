namespace Domain.Infrastructure.Messaging
{
    public interface IQuery<T>
        where T : IQueryResponse
    {
    }
}