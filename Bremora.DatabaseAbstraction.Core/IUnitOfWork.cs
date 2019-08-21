using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Core {
    public interface IUnitOfWork : IDisposable {
        IAccess Access { get; }
        IAccess StartTransaction();
        Task CommitTransaction();
        Task BulkInsert<T>(IEnumerable<T> entities, CancellationToken token = default);
    }
}