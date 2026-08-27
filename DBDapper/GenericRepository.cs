using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DBDapper
{
    /// <summary>
    /// Dapper vaparine generic query/execute methods.
    /// SQL text tamare j lakhvani rehse (Dapper micro-ORM che, full ORM nathi),
    /// pan connection open/close ane mapping automatic thai jashe - koi j database ma.
    /// </summary>
    public class GenericRepository
    {
        private readonly IDbConnectionFactory _factory;

        public GenericRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using IDbConnection conn = _factory.CreateConnection();
            return await conn.QueryAsync<T>(sql, param);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null)
        {
            using IDbConnection conn = _factory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using IDbConnection conn = _factory.CreateConnection();
            return await conn.ExecuteAsync(sql, param);
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            using IDbConnection conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<T>(sql, param);
        }

        /// <summary>
        /// Result ne DataTable ma joitu hoy tyare (jem ke DataGridView.DataSource
        /// direct bindh karvu hoy, ke model class banavi nathi tyare) aa method vaparo.
        /// </summary>
        public async Task<DataTable> QueryDataTableAsync(string sql, object? param = null)
        {
            using IDbConnection conn = _factory.CreateConnection();
            using var reader = await conn.ExecuteReaderAsync(sql, param);
            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        /// <summary>
        /// Multiple queries/executes ek j connection + transaction ma karva hoy tyare vaparo.
        /// </summary>
        public IDbConnection OpenConnection()
        {
            var conn = _factory.CreateConnection();
            conn.Open();
            return conn;
        }
    }
}
