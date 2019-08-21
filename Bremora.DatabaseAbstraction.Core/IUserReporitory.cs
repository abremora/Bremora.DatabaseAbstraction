using Bremora.DatabaseAbstraction.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Core {
    /// <summary>A more specific interface for accessing users.</summary>
    public interface IUserReporitory {
        Task<User> Load(string id);
        Task<IEnumerable<User>> Page(int start = 0, int length = 1024);
        Task Store(User item);
        Task BulkInsert(IEnumerable<User> entities, CancellationToken token = default);
        Task Delete(User item);
        Task Delete(string id);
        Task<IEnumerable<User>> LoadStartingWith(string id);
        Task<IEnumerable<User>> QueryUserWhichEndsWith(string endsWith, int count);        
    }
}
