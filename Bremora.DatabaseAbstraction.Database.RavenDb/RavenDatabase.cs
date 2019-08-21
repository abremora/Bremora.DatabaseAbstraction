using Bremora.DatabaseAbstraction.Core;
using Raven.Client.Documents;

namespace Bremora.DatabaseAbstraction.Database.RavenDb {
    public class RavenDatabase : IDatabase {
        private bool _disposedValue = false;

        public RavenDatabase(IDocumentStore database) {
            Database = database;
        }

        public IDocumentStore Database { get; }

        public IUnitOfWork CreateUnitOfWork() {
            return new RavenDbUnitOfWork(Database);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    Database?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}