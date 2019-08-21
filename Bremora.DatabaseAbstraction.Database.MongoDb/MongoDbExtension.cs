using MongoDB.Driver;

namespace Bremora.DatabaseAbstraction.Database.MongoDb {
    public static class MongoDbExtension {
        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase database) {
            return database.GetCollection<T>(nameof(T));
        }
    }
}
