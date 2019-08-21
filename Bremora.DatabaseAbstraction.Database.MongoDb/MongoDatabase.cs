using Bremora.DatabaseAbstraction.Core;
using MongoDB.Driver;

namespace Bremora.DatabaseAbstraction.Database.MongoDb {
    public class MongoDatabase : IDatabase {
        public MongoDatabase(IMongoDatabase database) {
            Database = database;
        }

        public IMongoDatabase Database { get; }

        public IUnitOfWork CreateUnitOfWork() {
            return new MongoDbUnitOfWork(Database);
        }

        public void Dispose() {

        }
    }
}