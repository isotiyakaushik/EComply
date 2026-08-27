using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;

namespace DBDapper
{
    /// <summary>
    /// Ek j factory jethi tamaru app DatabaseType change karine
    /// SQL Server / Oracle / MySQL / SQLite vachche switch kari shake,
    /// koi bija code ma change karya vagar.
    /// </summary>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DatabaseType DatabaseType { get; }

        public DbConnectionFactory(DatabaseType databaseType, string connectionString)
        {
            DatabaseType = databaseType;
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return DatabaseType switch
            {
                DatabaseType.SqlServer => new SqlConnection(_connectionString),
                DatabaseType.Oracle => new OracleConnection(_connectionString),
                DatabaseType.MySql => new MySqlConnection(_connectionString),
                DatabaseType.Sqlite => new SqliteConnection(_connectionString),
                _ => throw new NotSupportedException($"Database type '{DatabaseType}' supported nathi.")
            };
        }
    }
}
