using System;
using System.Data;
using System.Data.SQLite;

namespace Hangfire.Sqlite
{
    class Storage :IDisposable
    {
        private readonly KeySelector _keySelector;

        public Storage(SQLiteConnection connection)
        {
            ApplySchema(CREATE_DATABASE_SCRIPT, connection);
            _keySelector = new KeySelector(connection);
        }

        private static void ApplySchema(string databaseScript, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(databaseScript, connection))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }
        
        private const string CREATE_DATABASE_SCRIPT =
           @"
CREATE TABLE [objects] (
    [id] GUID PRIMARY KEY,
    [data] BLOB
);

CREATE TABLE IF NOT EXISTS [job] (  
  [id] INTEGER PRIMARY KEY,
  [stateid] INTEGER NULL,
  [statename] TEXT NULL,
  [invocationdata] TEXT NOT NULL,
  [arguments] TEXT NOT NULL,
  [createdat] TIMESTAMP NOT NULL,
  [expireat] TIMESTAMP NULL,
  PRIMARY KEY ([id])
); 

CREATE TABLE [files] (
    [id] GUID PRIMARY KEY,
    [data] BLOB
);
";

        public void Dispose()
        {
            _keySelector.Dispose();
        }

        public string GetFirstByLowestScoreFromSet(string key, double fromScore, double toScore)
        {
            return _keySelector.Select(key, fromScore, toScore);
        }
    }

    class KeySelector : IDisposable
    {
        private readonly SQLiteCommand _command;
        private readonly SQLiteParameter _key;
        private readonly SQLiteParameter _from;
        private readonly SQLiteParameter _to;

        public KeySelector(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(connection)
            {
                CommandText = @"select top 1 Value from HangFire.[Set] where [Key] = :key and Score between :from and :to order by Score"
            };
            _key = new SQLiteParameter { ParameterName = "key", DbType = DbType.String };
            _from = new SQLiteParameter { ParameterName = "from", DbType = DbType.Double };
            _to = new SQLiteParameter { ParameterName = "to", DbType = DbType.Double };
            _command.Parameters.Add(_key);
            _command.Parameters.Add(_from);
            _command.Parameters.Add(_to);
        }

        public string Select(string key, double fromScore, double toScore)
        {
            _key.Value = key;
            _from.Value = fromScore;
            _to.Value = toScore;
            var reader = _command.ExecuteScalar();
            if (reader == null)
                return string.Empty;
            return (string)reader;
        }

        public void Dispose()
        {
            _command.Dispose();
        }
    }
}
