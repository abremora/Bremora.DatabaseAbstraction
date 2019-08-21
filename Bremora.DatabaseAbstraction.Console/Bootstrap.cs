using Autofac;
using Bremora.DatabaseAbstraction.Core;
using Bremora.DatabaseAbstraction.Database.MongoDb;
using Bremora.DatabaseAbstraction.Database.RavenDb;

namespace Bremora.DatabaseAbstraction.Console {
    public class Bootstrap {
        public static IContainer Setup(IDatabase database) {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(database).As<IDatabase>();
            builder.RegisterType<UserService>();

            if (database is RavenDatabase) {

                builder.RegisterInstance(((RavenDatabase)database).Database);
                builder.RegisterType<RavenDbUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
                builder.RegisterType<RavenDbAccess>().As<IAccess>();
                builder.RegisterType<RavenUserRepository>().As<IUserReporitory>();
            }
            else if (database is MongoDatabase) {
                builder.RegisterInstance(((MongoDatabase)database).Database);
                builder.RegisterType<MongoDbUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
                builder.RegisterType<MongoDbAccess>().As<IAccess>();
                builder.RegisterType<MongoUserRepository>().As<IUserReporitory>();
            }

            return builder.Build();
        }
    }
}