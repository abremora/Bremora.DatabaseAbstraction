using Bremora.DatabaseAbstraction.Core;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Database.RavenDb {
    public class RavenDbUnitOfWork : IUnitOfWork {
        private bool disposedValue = false;
        private IAsyncDocumentSession _session;

        internal IDocumentStore Store { get; }
        public IAccess Access { get; set; }

        public RavenDbUnitOfWork(IDocumentStore store) {
            Store = store;
        }

        public async Task BulkInsert<T>(IEnumerable<T> entities, CancellationToken token = default) {
            using (var bulk = Store.BulkInsert(token: token)) {
                foreach (var entity in entities) {
                    await bulk.StoreAsync(entity);
                }
            }
        }

        public IAccess StartTransaction() {
            _session = Store.OpenAsyncSession();
            Access = new RavenDbAccess(_session);

            return Access;
        }

        public async Task CommitTransaction() {
            await _session.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _session?.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose() {
            Dispose(true);
        }
    }
}