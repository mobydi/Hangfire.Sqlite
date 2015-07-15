using System;
using Hangfire.Storage;

namespace Hangfire.Sqlite
{
    class SqliteFetchedJob: IFetchedJob
    {
        public void Dispose()
        {
            
        }

        public void RemoveFromQueue()
        {
            
        }

        public void Requeue()
        {
            
        }

        public string JobId
        {
            get
            {
                return String.Empty;
            }
        }
    }
}