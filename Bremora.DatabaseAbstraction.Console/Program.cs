using Bremora.DatabaseAbstraction.Console.Dialogs;
using Bremora.DatabaseAbstraction.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Console {
    class Program {
        static async Task Main(string[] args) {
            System.Console.WriteLine("Database Abstraction Sample");
            System.Console.WriteLine();

            IDatabase database = null;
            try {
                var appConfig = LoadConfigurationRoot();


                database = SelectDatabaseDialog.Run(appConfig);
                if (database == null) return;
                var container = Bootstrap.Setup(database);

                await new ActionDialog(container).Run();
            }
            catch (Exception ex) {
                System.Console.WriteLine($"ERROR: {ex.Message}");
                System.Console.ReadLine();
            }
            finally {
                database?.Dispose();
            }
        }

        private static IConfigurationRoot LoadConfigurationRoot() {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}