using Autofac;
using AutoFixture;
using Bremora.DatabaseAbstraction.Core;
using Bremora.DatabaseAbstraction.Core.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Console.Dialogs {
    public class ActionDialog {
        private Fixture _fixture = new Fixture();
        private readonly IContainer _container;

        public ActionDialog(IContainer container) {
            _container = container;
        }

        public async Task Run() {
            while (true) {
                System.Console.WriteLine();
                System.Console.WriteLine("Operations:");
                System.Console.WriteLine("[1] Create dummy data");
                System.Console.WriteLine("[2] Read data");
                System.Console.WriteLine("[3] Page data");
                System.Console.WriteLine("[4] Update data");
                System.Console.WriteLine("[5] Query data");
                System.Console.WriteLine("[x] Exit");
                System.Console.WriteLine();
                System.Console.Write("> ");

                var value = System.Console.ReadLine().ToLower();

                switch (value) {
                    case "1":
                        using (var scope = _container.BeginLifetimeScope()) {
                            var database = scope.Resolve<IDatabase>();
                            await Create(database);
                        }
                        break;
                    case "2":
                        using (var scope = _container.BeginLifetimeScope()) {
                            var database = scope.Resolve<IDatabase>();
                            await Read(database);
                        }
                        break;
                    case "3":
                        using (var scope = _container.BeginLifetimeScope()) {
                            var database = scope.Resolve<IDatabase>();
                            await Page(database);
                        }
                        break;
                    case "4":
                        using (var scope = _container.BeginLifetimeScope()) {
                            var database = scope.Resolve<IDatabase>();
                            await Update(database);
                        }
                        break;
                    case "5":
                        await Query();
                        break;
                    case "x":
                        return;
                }
            }
        }

        private async Task Create(IDatabase database) {
            System.Console.WriteLine("Create 1024 records.");
            var users = _fixture.CreateMany<User>(1024);

            using (var uow = database.CreateUnitOfWork()) {
                await uow.BulkInsert(users);
            }

            System.Console.WriteLine("Records created.");
        }

        private async Task Read(IDatabase database) {
            System.Console.WriteLine("Read by Id:");
            System.Console.Write("> ");
            var id = System.Console.ReadLine();
            if (string.IsNullOrEmpty(id)) {
                System.Console.WriteLine("No valid id." + Environment.NewLine);
                return;
            }

            using (var uow = database.CreateUnitOfWork()) {
                var access = uow.StartTransaction();
                var user = await access.LoadAsync<User>(id);
                System.Console.WriteLine("Result:");
                PrintResult(user);
            }
        }

        private async Task Page(IDatabase database) {
            var pageSize = 5;
            var page = -1;
            var continueLoop = true;
            do {
                page++;
                System.Console.WriteLine();
                System.Console.WriteLine($"Read next {pageSize} records (Page: {page})");

                var counter = 0;
                using (var uow = database.CreateUnitOfWork()) {
                    var access = uow.StartTransaction();
                    var results = await access.PageAsync<User>(page, 5);
                    foreach (var result in results) {
                        counter++;
                        System.Console.WriteLine($"Result {page * pageSize + counter}:");
                        PrintResult(result);
                    }
                }

                if (counter > 0) {
                    System.Console.WriteLine();
                    System.Console.WriteLine("Do you want to continue? (y/n)");
                    System.Console.Write("> ");
                    continueLoop = System.Console.ReadLine()?.ToLower() == "y";
                }
                else {
                    System.Console.WriteLine("No results found.");
                    continueLoop = false;
                }
            }
            while (continueLoop);
        }

        private async Task Update(IDatabase database) {
            string userId = null;
            using (var uow = database.CreateUnitOfWork()) {
                var access = uow.StartTransaction();
                var user = (await access.PageAsync<User>(0, 1)).FirstOrDefault();

                System.Console.WriteLine();
                System.Console.WriteLine("Loaded:");
                PrintResult(user);
                if (user != null) {
                    System.Console.WriteLine("Update Name:");
                    System.Console.Write("> ");
                    user.Name = System.Console.ReadLine();
                    userId = user.Id;

                    // Same databases do not support UoW directly.
                    // So we update explicitly.
                    await access.UpdateAsync(user);
                }
                else {
                    System.Console.WriteLine("No data found.");
                    return;
                }

                await uow.CommitTransaction();
            }

            if (userId != null) {
                System.Console.WriteLine();
                System.Console.WriteLine("Load document again:");

                using (var uow = database.CreateUnitOfWork()) {
                    var access = uow.StartTransaction();
                    var user = await access.LoadAsync<User>(userId);
                    PrintResult(user);
                }
            }
        }

        private async Task Query() {
            System.Console.WriteLine("Querys 5 users whose Id end with '1'");

            using (var scope = _container.BeginLifetimeScope()) {
                var uow = scope.Resolve<IUnitOfWork>();
                uow.StartTransaction();

                var service = scope.Resolve<UserService>();
                var users = await service.QueryUsersWhoseIdEndWith("1", 5);

                foreach (var user in users) {
                    PrintResult(user);
                }
            }          
        }

        private static void PrintResult(User user) {
            string json = "[NULL]";
            if (user != null) {
                json = JsonConvert.SerializeObject(user);
            }

            System.Console.WriteLine("-> " + json);
        }
    }
}