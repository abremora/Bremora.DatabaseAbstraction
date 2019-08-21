using Bremora.DatabaseAbstraction.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Database.MongoDb {
    public class MongoDbAccess : IAccess {

        public IMongoDatabase Database { get; set; }

        public MongoDbAccess(IMongoDatabase database) {
            Database = database;
        }

        public async Task<T> LoadAsync<T>(string id) where T : IAggregateRoot {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await (await Database.GetCollection<T>().FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> PageAsync<T>(int start = 0, int length = 1024) where T : IAggregateRoot {
            return await Database.GetCollection<T>().AsQueryable()
                .Skip(start)
                .Take(length)
                .ToListAsync();
        }

        public async Task StoreAsync<T>(T item) where T : IAggregateRoot {
            await Database.GetCollection<T>().InsertOneAsync(item);
        }

        public async Task DeleteAsync<T>(T item) where T : IAggregateRoot {
            var filter = Builders<T>.Filter.Eq("Id", item.Id);
            await Database.GetCollection<T>().DeleteOneAsync(filter);
        }

        public async Task DeleteAsync<T>(string id) where T : IAggregateRoot {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await Database.GetCollection<T>().DeleteOneAsync(filter);
        }

        public async Task UpdateAsync<T>(T item) where T : IAggregateRoot {
           await Database.GetCollection<T>().ReplaceOneAsync<T>(t => t.Id == item.Id, item);
        }
    }
}