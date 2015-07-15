using System.Data.Common;
using System.Data.SQLite;
using Hangfire.Storage;

namespace Hangfire.Sqlite
{
    public class SqliteStorage : JobStorage
    {
        private readonly SQLiteConnection _connection;

        public SqliteStorage(string connectionString)
        {
            //_connection = new SQLiteConnection(connectionString);
            //_connection.Open();
        }

        public SqliteStorage(DbConnectionStringBuilder options)
        {
            //_connection = new SQLiteConnection();
            //_connection.ConnectionString = options.ConnectionString;
            //_connection.Open();
        }

        public override IMonitoringApi GetMonitoringApi()
        {
            return new SqliteMonitoringApi(_connection);
        }

        public override IStorageConnection GetConnection()
        {
            return new SqliteStorageConnection(_connection);
        }
    }
}
