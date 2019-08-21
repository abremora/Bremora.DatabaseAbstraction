namespace Bremora.DatabaseAbstraction.Core {
    public interface IDatabaseFactory {
        IDatabase Create();
    }
}