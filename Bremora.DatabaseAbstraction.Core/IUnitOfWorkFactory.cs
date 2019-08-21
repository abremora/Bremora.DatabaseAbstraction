namespace Bremora.DatabaseAbstraction.Core {
    public interface IUnitOfWorkFactory {
        IUnitOfWork Create();
    }
}