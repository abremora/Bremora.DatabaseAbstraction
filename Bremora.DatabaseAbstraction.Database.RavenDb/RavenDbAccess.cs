using Bremora.DatabaseAbstraction.Core;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Database.RavenDb {
    public class RavenDbAccess : IAccess {

        public IAsyncDocumentSession Session { get; private set; }

        public RavenDbAccess(IAsyncDocumentSession session) {
            Session = session;
        }

        public async Task<T> LoadAsync<T>(string id) where T : IAggregateRoot {
            return await Session.LoadAsync<T>(id);
        }

        public async Task<IEnumerable<T>> PageAsync<T>(int start = 0, int length = 1024) where T : IAggregateRoot {
            return await Session.Query<T>()
                .Skip(start)
                .Take(length)
                .ToListAsync();
        }

        public async Task StoreAsync<T>(T item) where T : IAggregateRoot {
            await Session.StoreAsync(item);
        }

        public Task DeleteAsync<T>(T item) where T : IAggregateRoot {
            Session.Delete(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync<T>(string id) where T : IAggregateRoot {
            Session.Delete(id);
            return Task.CompletedTask;
        }

        public Task UpdateAsync<T>(T item) where T : IAggregateRoot {
            // Not necessary because of change tracking within a session
            return Task.CompletedTask;
        }
    }
}