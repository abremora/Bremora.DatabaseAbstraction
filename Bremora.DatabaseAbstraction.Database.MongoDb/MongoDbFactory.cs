using Bremora.DatabaseAbstraction.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Bremora.DatabaseAbstraction.Database.MongoDb {
    public class MongoDbFactory : IDatabaseFactory {
        private readonly IConfigurationRoot _config;

        public MongoDbFactory(IConfigurationRoot config) {
            _config = config;
        }       

        public IDatabase Create() {
            var conn = _config.GetConnectionString("MongoDbConnection");
            var connectionString = new ConnectionString(conn);

            var client = new MongoClient(conn);
            var database = client.GetDatabase(connectionString.DatabaseName);
            return new MongoDatabase(database);
        }
    }
}