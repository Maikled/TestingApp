using TestingApp.Core.Models.Identity;

namespace TestingApp.Core.Models.Tests
{
    public class Source
    {
        public Guid ID { get; set; }
        public required User Owner { get; set; }
        public required string Name { get; set; }
    }
}
