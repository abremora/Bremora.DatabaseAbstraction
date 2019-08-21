using Bremora.DatabaseAbstraction.Core;
using Bremora.DatabaseAbstraction.Core.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Database.RavenDb {
    public class RavenUserRepository : IUserReporitory {
        readonly IAsyncDocumentSession _session;

        public RavenUserRepository(IUnitOfWork uow) {
            var ravenSession = uow.Access as RavenDbAccess;
            if (ravenSession == null) {
                throw new InvalidDataException($"Argument must be of type {nameof(RavenDbAccess)}");
            }
            _session = ravenSession.Session;
        }

        public async Task BulkInsert(IEnumerable<User> entities, CancellationToken token = default) {
            using (var bulk = _session.Advanced.DocumentStore.BulkInsert(token: token)) {
                foreach (var entity in entities) {
                    await bulk.StoreAsync(entity);
                }
            }
        }

        public Task Delete(User item) {
            _session.Delete(item);
            return Task.CompletedTask;
        }

        public Task Delete(string id) {
            _session.Delete(id);
            return Task.CompletedTask;
        }

        public async Task<User> Load(string id) {
            return await _session.LoadAsync<User>(id);
        }

        public async Task<IEnumerable<User>> LoadStartingWith(string id) {
            return await _session.Advanced.LoadStartingWithAsync<User>(id, pageSize: 1024);
        }

        public async Task<IEnumerable<User>> Page(int start = 0, int length = 1024) {
            return await _session.Query<User>()
                 .Skip(start)
                 .Take(length)
                 .ToListAsync();
        }

        public async Task<IEnumerable<User>> QueryUserWhichEndsWith(string endsWith, int count) {
            return await _session.Query<User>()
               .Where(x => x.Id.EndsWith(endsWith))
               .Take(count)
               .ToListAsync();
        }

        public async Task Store(User item) {
            await _session.StoreAsync(item);
        }
    }
}