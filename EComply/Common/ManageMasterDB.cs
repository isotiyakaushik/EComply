using DBDapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComply.Common
{
    public class ManageMasterDB
    {
        DbConnectionFactory factory;
        GenericRepository repo;
        public ManageMasterDB()
        {
            factory = new DbConnectionFactory(DatabaseType.Sqlite, Statics.MainConnectionString);
            repo = new GenericRepository(factory);
        }

        public async void Table()
        {
            try
            {
                await repo.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS CompanyMaster (id INTEGER PRIMARY KEY AUTOINCREMENT, gstin TEXT NOT NULL, trade_name TEXT NOT NULL, address TEXT NOT NULL, mobile_no TEXT, email TEXT, gst_user_name TEXT, gst_password TEXT, e_user_name TEXT, e_password TEXT)");

                await repo.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS CompanyDBMaster (id INTEGER PRIMARY KEY AUTOINCREMENT, gstin TEXT NOT NULL, db_type TEXT NOT NULL, db_connection_string TEXT NOT NULL)");

            }
            catch (Exception ex)
            {
                Error.HandleShow(ex);
            }
        }
    }
}
