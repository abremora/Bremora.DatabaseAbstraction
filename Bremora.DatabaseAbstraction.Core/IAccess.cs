using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Core {
    /// <summary>Introduce a simple generic interface for accessing data within a database.</summary>
    public interface IAccess {
        Task<T> LoadAsync<T>(string id) where T : IAggregateRoot;
        Task<IEnumerable<T>> PageAsync<T>(int start = 0, int length = 1024) where T : IAggregateRoot;       
        Task StoreAsync<T>(T item) where T : IAggregateRoot;
        Task UpdateAsync<T>(T item) where T : IAggregateRoot;
        Task DeleteAsync<T>(T item) where T : IAggregateRoot;
        Task DeleteAsync<T>(string id) where T : IAggregateRoot;
    }
}
