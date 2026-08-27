using System.Data;

namespace DBDapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        DatabaseType DatabaseType { get; }
    }
}
