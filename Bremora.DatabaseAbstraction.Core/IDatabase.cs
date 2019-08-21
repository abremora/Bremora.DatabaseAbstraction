using System;

namespace Bremora.DatabaseAbstraction.Core {
    public interface IDatabase : IDisposable {
        IUnitOfWork CreateUnitOfWork();
    }
}