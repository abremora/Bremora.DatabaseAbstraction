using Bremora.DatabaseAbstraction.Core;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Database.MongoDb {
    public class MongoDbUnitOfWork : IUnitOfWork {
        private bool disposedValue = false;
        private IClientSessionHandle _transaction;
        private IMongoDatabase _store;

        public IAccess Access { get; set; }

        public MongoDbUnitOfWork(IMongoDatabase store) {
            _store = store;
        }

        public async Task BulkInsert<T>(IEnumerable<T> entities, CancellationToken token = default) {
            var collection = _store.GetCollection<T>(nameof(T));
            await collection.InsertManyAsync(entities);
        }

        public IAccess StartTransaction() {
            _transaction = _store.Client.StartSession();
            _transaction.StartTransaction();
            Access = new MongoDbAccess(_store);
            return Access;                
        }

        public async Task CommitTransaction() {
            await _transaction.CommitTransactionAsync();
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _transaction?.Dispose();                    
                }
                disposedValue = true;
            }
        }
        public void Dispose() {
            Dispose(true);
        }
    }
}