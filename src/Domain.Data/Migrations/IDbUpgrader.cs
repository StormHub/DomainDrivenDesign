namespace Domain.Data.Migrations
{
    public interface IDbUpgrader
    {
        void Run(string connectionString);
    }
}