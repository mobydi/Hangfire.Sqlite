using System;
using Xunit;

namespace Hangfire.Sqlite.Test
{
    public class IntegrationTest
    {
        [Fact]
        void test()
        {
            JobStorage.Current = new SqliteStorage("connection_string");

            using (var server = new BackgroundJobServer())
            {
                server.Start();

                BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));

                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
