using Bremora.DatabaseAbstraction.Core;
using Bremora.DatabaseAbstraction.Core.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Database.MongoDb {
    public class MongoUserRepository : IUserReporitory {
        readonly IMongoCollection<User> _collection;

        public MongoUserRepository(IUnitOfWork uow) {
            var ravenSession = uow.Access as MongoDbAccess;
            if (ravenSession == null) {
                throw new InvalidDataException($"Argument must be of type {nameof(MongoDbAccess)}");
            }
            _collection = ravenSession.Database.GetCollection<User>();
        }

        public async Task BulkInsert(IEnumerable<User> entities, CancellationToken token = default) {
           await _collection.InsertManyAsync(entities);
        }

        public async Task Delete(User item) {
            var filter = Builders<User>.Filter.Eq("Id", item.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task Delete(string id) {
            var filter = Builders<User>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<User> Load(string id) {
            var filter = Builders<User>.Filter.Eq("Id", id);
            return await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> LoadStartingWith(string id) {          
            return await _collection.AsQueryable()
                .Where(x => x.Id.StartsWith(id))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> Page(int start = 0, int length = 1024) {
            return await _collection.AsQueryable()
                  .Skip(start)
                  .Take(length)
                  .ToListAsync();
        }

        public async Task<IEnumerable<User>> QueryUserWhichEndsWith(string endsWith, int count) {
            return await _collection.AsQueryable()
                .Where(x => x.Id.EndsWith(endsWith))
                .Take(count)
                .ToListAsync();
        }

        public async Task Store(User item) {
            await _collection.InsertOneAsync(item);
        }
    }
}