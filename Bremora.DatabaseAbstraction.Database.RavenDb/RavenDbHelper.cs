using System.Linq;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Bremora.DatabaseAbstraction.Database.RavenDb {
    public static class RavenDbHelper {
        public static DocumentStore CreateStore(string connectionString) {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            var server = builder["Url"] as string;
            var database = builder["Database"] as string;

            var store = new DocumentStore {
                Database = database,
                Urls = new[] { server }
            };
            store.Initialize();

            var databaseNames = store.Maintenance.Server.Send(new GetDatabaseNamesOperation(0, 1024));
            if (!databaseNames.Contains(database)) {
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
            }

            return store;
        }
    }
}