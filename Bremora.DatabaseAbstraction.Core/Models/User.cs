using System.Collections.Generic;

namespace Bremora.DatabaseAbstraction.Core.Models {
    public class User : IAggregateRoot {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> FavoriteColors { get; set; }
        public Address Address { get; set; }
    }
}