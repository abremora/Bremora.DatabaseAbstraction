using Bremora.DatabaseAbstraction.Core;
using Microsoft.Extensions.Configuration;

namespace Bremora.DatabaseAbstraction.Database.RavenDb {
    public class RavenDbFactory : IDatabaseFactory {
        private readonly IConfigurationRoot _config;

        public RavenDbFactory(IConfigurationRoot config) {
            _config = config;
        }

        public IDatabase Create() {
            var conn = _config.GetConnectionString("RavenDbConnection");
            var store = RavenDbHelper.CreateStore(conn);
            return new RavenDatabase(store);
        }
    }
}