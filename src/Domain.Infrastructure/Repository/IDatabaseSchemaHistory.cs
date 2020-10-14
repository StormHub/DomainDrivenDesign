using System.Threading.Tasks;

namespace Domain.Infrastructure.Repository
{
    public interface IDatabaseSchemaHistory
    {
        string Name { get; }

        Task<string> GetLastMigration();
    }
}
