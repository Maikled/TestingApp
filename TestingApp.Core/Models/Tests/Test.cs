using TestingApp.Core.Models.Identity;

namespace TestingApp.Core.Models.Tests
{
    public class Test
    {
        public Guid ID { get; set; }
        public required User Owner { get; set; }
        public required Source Source { get; set; }
        public required string Name { get; set; }
        public required string InputData { get; set; }
        public required string OutputData { get; set; }
    }
}
