using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Threading;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;

namespace Hangfire.Sqlite
{
    public class SqliteStorageConnection : IStorageConnection
    {
        private readonly SQLiteConnection _connection;
        private readonly SqliteStorageOptions _options;

        public SqliteStorageConnection(SQLiteConnection connection) 
            : this(connection, new SqliteStorageOptions()) {  }

        public SqliteStorageConnection(SQLiteConnection connection, SqliteStorageOptions options)
        {
            _connection = connection;
            _options = options;
        }

        public void Dispose()
        {
            
        }

        public IWriteOnlyTransaction CreateWriteTransaction()
        {
            return new SqliteWriteOnlyTransaction(_connection);
        }

        public IDisposable AcquireDistributedLock(string resource, TimeSpan timeout)
        {
            return new SqliteDistributedLock(resource, timeout);
        }

        public string CreateExpiredJob(Job job, IDictionary<string, string> parameters, DateTime createdAt, TimeSpan expireIn)
        {
            var invocationData = InvocationData.Serialize(job);
            
            var jsonJob = JobHelper.ToJson(invocationData);
            var jsonCreatedAt = JobHelper.SerializeDateTime(createdAt);
            //var jsonExpireIn = JobHelper.SerializeDateTime(expireIn);

            var jsonParameters = JobHelper.ToJson(parameters);
            return String.Empty;
        }

        public IFetchedJob FetchNextJob(string[] queues, CancellationToken cancellationToken)
        {
            if (queues == null || queues.Length == 0) 
                throw new ArgumentNullException("queues");

            SqliteFetchedJob fetchedJob;
            var currentQueryIndex = 0;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                fetchedJob = _connection.JobQueue
                    .FindAndModify(new FindAndModifyArgs
                    {
                        Query = Query.And(fetchConditions[currentQueryIndex], Query<JobQueueDto>.In(_ => _.Queue, queues)),
                        Update = Update<JobQueueDto>.Set(_ => _.FetchedAt, _connection.GetServerTimeUtc()),
                        VersionReturned = FindAndModifyDocumentVersion.Modified,
                        Upsert = false
                    })
                    .GetModifiedDocumentAs<JobQueueDto>();

                if (fetchedJob == null)
                {
                    if (currentQueryIndex == fetchConditions.Length - 1)
                    {
                        cancellationToken.WaitHandle.WaitOne(_options.QueuePollInterval);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }

                currentQueryIndex = (currentQueryIndex + 1) % fetchConditions.Length;
            }
            while (fetchedJob == null);

            return new SqliteFetchedJob(_connection, fetchedJob.Id, fetchedJob.JobId.ToString(CultureInfo.InvariantCulture), fetchedJob.Queue);
        }

        public void SetJobParameter(string id, string name, string value)
        {
            throw new NotImplementedException();
        }

        public string GetJobParameter(string id, string name)
        {
            throw new NotImplementedException();
        }

        public JobData GetJobData(string jobId)
        {
            throw new NotImplementedException();
        }

        public StateData GetStateData(string jobId)
        {
            throw new NotImplementedException();
        }

        public void AnnounceServer(string serverId, ServerContext context)
        {
            
        }

        public void RemoveServer(string serverId)
        {
            
        }

        public void Heartbeat(string serverId)
        {
      
        }

        public int RemoveTimedOutServers(TimeSpan timeOut)
        {
            return 0;
        }

        public HashSet<string> GetAllItemsFromSet(string key)
        {
            throw new NotImplementedException();
        }

        public string GetFirstByLowestScoreFromSet(string key, double fromScore, double toScore)
        {
            return String.Empty;
            //return _connection.GetFirstByLowestScoreFromSet(key, fromScore, toScore );
        }

        public void SetRangeInHash(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetAllEntriesFromHash(string key)
        {
            throw new NotImplementedException();
        }
    }
}