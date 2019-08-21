using AutoFixture;
using Bremora.DatabaseAbstraction.Core;
using Bremora.DatabaseAbstraction.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bremora.DatabaseAbstraction.Console {
    public class UserService {
        private readonly IUserReporitory _repository;
        private Fixture _fixture = new Fixture();

        public UserService(IUserReporitory repository) {
            _repository = repository;
        }

        public async Task CreateDummyUser(int count) {
            System.Console.WriteLine($"Create {count} dummy users.");
            var users = _fixture.CreateMany<User>(count);

            await _repository.BulkInsert(users);

            System.Console.WriteLine("Records created.");
        }

        public async Task<IEnumerable<User>> QueryUsersWhoseIdEndWith(string endsWith, int count) {
            return await _repository.QueryUserWhichEndsWith(endsWith, count);
        }
    }
}