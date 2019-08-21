using Bremora.DatabaseAbstraction.Database.MongoDb;
using Microsoft.Extensions.Configuration;
using Bremora.DatabaseAbstraction.Core;
using Bremora.DatabaseAbstraction.Database.RavenDb;

namespace Bremora.DatabaseAbstraction.Console.Dialogs {
    internal static class SelectDatabaseDialog {

        public static IDatabase Run(IConfigurationRoot config) {
            while (true) {
                System.Console.WriteLine("Select a supported database:");
                System.Console.WriteLine("[1] RavenDB v4");
                System.Console.WriteLine("[2] MongoDb");
                System.Console.WriteLine("[x] Exit");
                System.Console.WriteLine();
                System.Console.Write("> ");

                var value = System.Console.ReadLine().ToLower();
                System.Console.Clear();

                switch (value) {
                    case "1":
                        return new RavenDbFactory(config).Create();
                    case "2":
                        return new MongoDbFactory(config).Create();
                    case "x":
                        return null;
                }
            }
        }
    }
}